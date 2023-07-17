using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core.Modules;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Thread;
using Propeus.Modulo.Util.Tipos;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public sealed class ModuleManagerCore : BaseModel, IModuleManager
    {

        ///<inheritdoc/>
        public DateTime LastUpdate { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int InitializedModules { get; private set; }


        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        internal ModuleManagerCore()
        {
            StartDate = DateTime.Now;

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


        }

        private readonly CancellationTokenSource _cancellationToken = new();

        //K:Id | V:moduleInstance
        readonly ConcurrentDictionary<string, IModuleType> modules = new ConcurrentDictionary<string, IModuleType>();

        internal void RegisterTaskJobs(TaskJobModule taskJobModule)
        {
            taskJobModule.RegisterJob((ct) =>
            {
                var modulesOff = modules.Where(x => x.Value.IsDeleted);
                foreach (var item in modulesOff)
                {
                    RemoveModule(item.Key);
                }

            }, $"{this.GetType().FullName}::Auto_Remove_Modules", TimeSpan.FromSeconds(1));
        }

        ///<inheritdoc/>
        public DateTime StartDate { get; private set; }

        ///<inheritdoc/>
        public T CreateModule<T>() where T : IModule
        {
            return (T)CreateModule(typeof(T));
        }
        ///<inheritdoc/>
        public IModule CreateModule(string nomeModulo)
        {
            Type result = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();
            foreach (var item in assemblies)
            {
                result = item.GetTypes().FirstOrDefault(x => x.Name == nomeModulo);
                if (result != null)
                {
                    break;
                }
            }

            return result == null
                ? throw new ModuleTypeNotFoundException(string.Format(Constantes.ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : CreateModule(result);
        }
        ///<inheritdoc/>
        public IModule CreateModule(Type moduloType)
        {
            if (moduloType is null)
            {
                throw new ArgumentNullException(nameof(moduloType));
            }
            moduloType = ResolverContrato(moduloType);

            ConstructorInfo ctor = moduloType.GetConstructors().MaxBy(x => x.GetParameters().Length);
            if (ctor is null)
            {
                throw new ModuleBuilderAbsentException(Constantes.ERRO_CONSTRUTOR_NAO_ENCONTRADO);
            }

            ParameterInfo[] paramCtor = ctor.GetParameters();
            object[] args = new object[paramCtor.Length];

            for (int i = 0; i < paramCtor.Length; i++)
            {
                if (!paramCtor[i].ParameterType.IsAssignableTo(typeof(IModuleManager)) && (paramCtor[i].ParameterType.IsAssignableTo(typeof(IModule)) || paramCtor[i].ParameterType.PossuiAtributo<ModuleContractAttribute>()))
                {
                    if (!ExistsModule(paramCtor[i].ParameterType))
                    {
                        if (paramCtor[i].IsOptional)
                        {
                            try
                            {
                                args[i] = Convert.ChangeType(CreateModule(paramCtor[i].ParameterType), paramCtor[i].ParameterType);
                            }
                            catch (ModuleTypeNotFoundException)
                            {
                                args[i] = paramCtor[i].ParameterType.Default();
                            }
                        }
                        else
                        {
                            args[i] = CreateModule(paramCtor[i].ParameterType)/*.To(paramCtor[i].ParameterType)*/;
                        }
                    }
                    else
                    {
                        args[i] = GetModule(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                }
                else if (paramCtor[i].ParameterType.IsAssignableTo(typeof(IModuleManager)))
                {
                    IModuleType gen = modules
                        .Where(x => !x.Value.IsDeleted)
                        .Select(x => x.Value)
                        .FirstOrDefault(x => x.Module is IModuleManager);
                    args[i] = (gen?.Module as IModuleManager) ?? this;
                }
                else
                {
                    //TODO: E para IsNulable?
                    if (paramCtor[i].HasDefaultValue)
                    {
                        args[i] = paramCtor[i].DefaultValue;
                        continue;
                    }
                    else if (paramCtor[i].IsOptional)
                    {
                        continue;
                    }
                    else if (paramCtor[i].IsNullable())
                    {
                        continue;
                    }

                    throw new ModuleTypeInvalidException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Module, Contrato ou ModuleManagerCore");
                }
            }



            var modulo = (IModule)Activator.CreateInstance(moduloType, args);
            Register(modulo);
            InitializedModules++;
            return modulo;

        }

        ///<inheritdoc/>
        public bool ExistsModule(Type moduleType)
        {
            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            try
            {
                moduleType = ResolverContrato(moduleType);
                var moduloInstancia = modules.Values.FirstOrDefault(x => x.Name == moduleType.Name);

                if (moduloInstancia is null)
                {
                    return false;
                }
                if (moduloInstancia.IsDeleted || moduloInstancia.IsCollected)
                {
                    return false;
                }

                return true;
            }
            catch (ModuleTypeNotFoundException)
            {
                return false;
            }

        }
        ///<inheritdoc/>
        public bool ExistsModule(IModule modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : ExistsModule(modulo.Id);
        }
        ///<inheritdoc/>
        public bool ExistsModule(string idModule)
        {
            if (string.IsNullOrEmpty(idModule))
                throw new ArgumentNullException(nameof(idModule));
            else
            {
                modules.TryGetValue(idModule, out var moduloInformacao);
                if (moduloInformacao is null)
                {
                    return false;
                }
                if (moduloInformacao.IsCollected || moduloInformacao.IsDeleted)
                {
                    return false;
                }
                return true;
            }
        }
        ///<inheritdoc/>
        public IModule GetModule(Type moduleType)
        {
            moduleType = ResolverContrato(moduleType);
            var moduloInstancia = modules.Values.FirstOrDefault(x => x.Name == moduleType.Name);

            if (moduloInstancia is null)
            {
                throw new ModuleNotFoundException("Module nao encontrado");
            }
            if (moduloInstancia.IsDeleted || moduloInstancia.IsCollected)
            {
                throw new ModuleDisposedException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, moduloInstancia.IdModule));
            }


            return moduloInstancia.Module;
        }
        ///<inheritdoc/>
        public T GetModule<T>() where T : IModule
        {
            return (T)GetModule(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleNotFoundException">Instancia do moduleInstance nao foi inicializado</exception>
        public IModule GetModule(string idModule)
        {
            if (string.IsNullOrEmpty(idModule))
            {
                throw new ArgumentException($"'{nameof(idModule)}' não pode ser nulo nem vazio.", nameof(idModule));
            }

            if (!ExistsModule(idModule))
            {
                throw new ModuleNotFoundException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, idModule));
            }

            modules.TryGetValue(idModule, out var info);
            return info.IsDeleted ? throw new ModuleDisposedException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, idModule)) : info.Module;
        }
        ///<inheritdoc/>
        public void RemoveModule<T>(T moduleInstance) where T : IModule
        {
            if (modules.TryRemove(moduleInstance.Id, out IModuleType target) && !target.IsCollected)
            {
                target.Dispose();
                InitializedModules--;
            }


        }
        ///<inheritdoc/>
        public void RemoveModule(string idModule)
        {
            if (string.IsNullOrEmpty(idModule))
            {
                throw new ArgumentException($"'{nameof(idModule)}' não pode ser nulo nem vazio.", nameof(idModule));
            }

            if (modules.TryRemove(idModule, out IModuleType target) && !target.IsDeleted)
            {
                target.Dispose();
                InitializedModules--;
            }
            else
            {
                throw new ModuleNotFoundException("Module nao encontrdo");
            }


        }
        ///<inheritdoc/>
        public void RemoveAllModules()
        {
            foreach (KeyValuePair<string, IModuleType> item in modules.Where(x => !x.Value.IsCollected))
            {
                RemoveModule(item.Key);
            }


            modules.Clear();

        }

        ///<inheritdoc/>
        public T RecycleModule<T>(T moduleInstance) where T : IModule
        {
            return (T)RecycleModule(moduleInstance.Id);
        }
        ///<inheritdoc/>
        public IModule RecycleModule(string idModule)
        {

            if (modules.TryGetValue(idModule, out var moduloTipo))
            {
                var moduleType = moduloTipo.ModuleType;
                RemoveModule(idModule);
                return CreateModule(moduleType);
            }



            throw new ModuleNotFoundException(Constantes.ERRO_MODULO_NEW_REINICIAR);

        }


        ///<inheritdoc/>
        public async Task KeepAliveAsync()
        {
            await Task.Run(async () =>
              {
                  while (!_cancellationToken.IsCancellationRequested)
                  {
                      await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
                  }

              }, _cancellationToken.Token).ConfigureAwait(false);
        }
        ///<inheritdoc/>
        public async Task KeepAliveModuleAsync(IModule moduleInstance)
        {
            modules[moduleInstance.Id].KeepAliveModule(true);
            await Task.CompletedTask;
        }

        ///<inheritdoc/>
        public IEnumerable<IModule> ListAllModules()
        {
            return modules.Select(x => x.Value.Module);
        }

        private void Register(IModule modulo)
        {
            if (modulo.IsSingleInstance && ExistsModule(modulo.GetType()))
            {
                throw new ModuleSingleInstanceException(Constantes.ERRO_MODULO_INSTANCIA_UNICA);
            }


            if (!modules.ContainsKey(modulo.Id))
            {
                modules.TryAdd(modulo.Id, new ModuloTipo(modulo));
            }
        }

        ///<inheritdoc/>
        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Dispose();
            e.Cancel = true;
        }
        ///<inheritdoc/>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            this.Dispose();
        }

        ///<inheritdoc/>
        private Type ResolverContrato(Type modulo)
        {
            if (!modulo.IsInterface && !modulo.IsClass)
            {
                throw new ModuleTypeInvalidException(Constantes.ERRO_TIPO_INVALIDO);
            }

            if (modulo.IsInterface)
            {
                ModuleContractAttribute attr = modulo.GetAttributeContractModule() ?? throw new ModuleContractNotFoundException(Constantes.ERRO_TIPO_NAO_MARCADO);

                if (!attr.IsValid)
                {
                    throw new ModuleContractInvalidException("O atributo deve possui um nome ou tipo do modulo");
                }

                modulo = attr.ModuleType;
                if (modulo is null)
                {
                    foreach (var item in modules)
                    {
                        if(item.Value.Name == attr.ModuleName)
                        {
                            modulo = item.Value.ModuleType;
                            break;
                        }
                    }
                }
              
                if (modulo is null)
                {
                    throw new ModuleTypeNotFoundException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, attr.ModuleName));
                }

            }

            if (modulo.IsClass)
            {
                if (!modulo.IsAssignableTo(typeof(IModule)))
                {
                    throw new ModuleTypeInvalidException(Constantes.ERRO_TIPO_NAO_HERDADO);
                }

                if (modulo.GetModuleAttribute() is null)
                {
                    throw new ModuleTypeInvalidException(Constantes.ERRO_TIPO_NAO_MARCADO);
                }
            }

            return modulo;
        }


        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
            }
            base.Dispose(disposing);

        }
        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());
            _ = stringBuilder.Append("Ultima atualização: ").Append(LastUpdate).AppendLine();
            _ = stringBuilder.Append("Modules inicializados: ").Append(InitializedModules).AppendLine();
            return stringBuilder.ToString();

        }


    }
}
