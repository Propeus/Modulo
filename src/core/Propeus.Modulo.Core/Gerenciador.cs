using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Thread;
using Propeus.Modulo.Util.Tipos;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public sealed class Gerenciador : ModeloBase, IGerenciador
    {
        private static Gerenciador _atual;

        ///<inheritdoc/>
        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int ModulosInicializados { get; private set; }


        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        private Gerenciador()
        {
            semaphoreSlimCreate = new SemaphoreSlim(1, 1);
            DataInicio = DateTime.Now;

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            TaskJob.GetTasker().RegisterJob((ct) =>
            {
                var modulesOff = modules.Where(x => x.Value.Elimindado);
                foreach (var item in modulesOff)
                {
                    Remover(item.Value.Id);
                }

            }, $"{this.GetType().FullName}::Auto_Remove_Modules", TimeSpan.FromSeconds(1));
        }

        private readonly CancellationTokenSource _cancellationToken = new();
        private SemaphoreSlim semaphoreSlimCreate;

        //K:Id | V:modulo
        ConcurrentDictionary<string, IModuloTipo> modules = new ConcurrentDictionary<string, IModuloTipo>();


        ///<inheritdoc/>
        public static IGerenciador Atual
        {
            get
            {
                if (_atual is null || _atual.Estado == Estado.Desligado || _atual.disposedValue)
                {
                    _atual = new Gerenciador();
                }

                return _atual;
            }
        }

        ///<inheritdoc/>
        public DateTime DataInicio { get; private set; }

        ///<inheritdoc/>
        public T Criar<T>() where T : IModulo
        {
            return (T)Criar(typeof(T));
        }
        ///<inheritdoc/>
        public IModulo Criar(string nomeModulo)
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
                ? throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : Criar(result);
        }
        ///<inheritdoc/>
        public IModulo Criar(Type moduloType)
        {
            if (moduloType is null)
            {
                throw new ArgumentNullException(nameof(moduloType));
            }
            moduloType = ResolverContrato(moduloType);
            //TODO:Existe o modulo no provider, porem ele ja foi eliminado



            ConstructorInfo ctor = moduloType.GetConstructors().MaxBy(x => x.GetParameters().Length);
            if (ctor is null)
            {
                throw new ModuloConstrutorAusenteException(Constantes.ERRO_CONSTRUTOR_NAO_ENCONTRADO);
            }

            ParameterInfo[] paramCtor = ctor.GetParameters();
            object[] args = new object[paramCtor.Length];

            for (int i = 0; i < paramCtor.Length; i++)
            {
                if (!paramCtor[i].ParameterType.IsAssignableTo(typeof(IGerenciador)) && (paramCtor[i].ParameterType.IsAssignableTo(typeof(IModulo)) || paramCtor[i].ParameterType.PossuiAtributo<ModuloContratoAttribute>()))
                {
                    if (!Existe(paramCtor[i].ParameterType))
                    {
                        if (paramCtor[i].IsOptional)
                        {
                            try
                            {
                                args[i] = Convert.ChangeType(Criar(paramCtor[i].ParameterType), paramCtor[i].ParameterType);
                            }
                            catch (TipoModuloNaoEncontradoException)
                            {
                                args[i] = paramCtor[i].ParameterType.Default();
                            }
                        }
                        else
                        {
                            args[i] = Criar(paramCtor[i].ParameterType)/*.To(paramCtor[i].ParameterType)*/;
                        }
                    }
                    else
                    {
                        args[i] = Obter(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                }
                else if (paramCtor[i].ParameterType.IsAssignableTo(typeof(IGerenciador)))
                {
                    IModuloTipo gen = modules
                        .Where(x => !x.Value.Elimindado)
                        .Select(x => x.Value)
                        .FirstOrDefault(x => x.Modulo is IGerenciador);
                    args[i] = (gen?.Modulo as IGerenciador) ?? this;
                }
                else
                {
                    //TODO: E para IsNulable?
                    if (paramCtor[i].IsOptional)
                    {
                        continue;
                    }
                    if (paramCtor[i].IsNullable())
                    {
                        continue;
                    }

                    throw new TipoModuloInvalidoException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Modulo, Contrato ou Gerenciador");
                }
            }



            var modulo = (IModulo)Activator.CreateInstance(moduloType, args);
            this.NotificarInformacao($"Modulo {modulo.Id} criado com sucesso");
            Register(modulo);
            return modulo;

        }

        ///<inheritdoc/>
        public bool Existe(Type moduleType)
        {
            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            try
            {
                moduleType = ResolverContrato(moduleType);
                var moduloInstancia = modules.Values.FirstOrDefault(x => x.Nome == moduleType.Name);

                if (moduloInstancia is null)
                {
                    return false;
                }
                if (moduloInstancia.Elimindado || moduloInstancia.Coletado)
                {
                    return false;
                }

                return true;
            }
            catch (TipoModuloNaoEncontradoException)
            {
                return false;
            }

        }
        ///<inheritdoc/>
        public bool Existe(IModulo modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : Existe(modulo.Id);
        }
        ///<inheritdoc/>
        public bool Existe(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            else
            {
                modules.TryGetValue(id, out var moduloInformacao);
                if (moduloInformacao is null)
                {
                    return false;
                }
                if (moduloInformacao.Coletado || moduloInformacao.Elimindado)
                {
                    return false;
                }
                return true;
            }
        }
        ///<inheritdoc/>
        public IModulo Obter(Type moduleType)
        {
            moduleType = ResolverContrato(moduleType);
            var moduloInstancia = modules.Values.FirstOrDefault(x => x.Nome == moduleType.Name);

            if (moduloInstancia is null)
            {
                throw new ModuloNaoEncontradoException("Modulo nao encontrado");
            }
            if (moduloInstancia.Elimindado || moduloInstancia.Coletado)
            {
                throw new ModuloDescartadoException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, moduloInstancia.IdModulo));
            }


            return moduloInstancia.Modulo;
        }
        ///<inheritdoc/>
        public T Obter<T>() where T : IModulo
        {
            return (T)Obter(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        public IModulo Obter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' não pode ser nulo nem vazio.", nameof(id));
            }

            if (!Existe(id))
            {
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, id));
            }

            modules.TryGetValue(id, out var info);
            return info.Elimindado ? throw new ModuloDescartadoException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, id)) : info.Modulo;
        }
        ///<inheritdoc/>
        public void Remover<T>(T modulo) where T : IModulo
        {
            if (modules.TryRemove(modulo.Id, out IModuloTipo target) && !target.Coletado)
            {
                target.Dispose();
                this.NotificarInformacao($"Modulo {modulo.Nome}::{modulo.Id} removido com sucesso");
            }
            else
            {
                this.NotificarAviso($"Modulo {modulo.Nome}::{modulo.Id} nao foi encontrado no provedor");
            }
        
        }
        ///<inheritdoc/>
        public void Remover(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' não pode ser nulo nem vazio.", nameof(id));
            }

            if (modules.TryRemove(id, out IModuloTipo target) && !target.Elimindado)
            {
             
                target.Dispose();
                this.NotificarInformacao($"Modulo {target.Nome}::{target.Id} removido com sucesso");
            }
            this.NotificarAviso($"Modulo {id} nao foi encontrado no provedor");
            throw new ModuloNaoEncontradoException("Modulo nao encontrdo");

        }
        ///<inheritdoc/>
        public void RemoverTodos()
        {
            foreach (KeyValuePair<string, IModuloTipo> item in modules.Where(x => !x.Value.Coletado))
            {
                this.NotificarInformacao($"Modulo {item.Value.Modulo.Nome}::{item.Value.Modulo.Id} removido com sucesso");
                item.Value.Dispose();
            }

            modules.Clear();
           
        }

        ///<inheritdoc/>
        public T Reciclar<T>(T modulo) where T : IModulo
        {
            return (T)Reciclar(modulo.Id);
        }
        ///<inheritdoc/>
        public IModulo Reciclar(string id)
        {
            if (modules.ContainsKey(id))
            {
                modules.TryGetValue(id, out var moduloTipo);
                if (modules.TryRemove(id, out IModuloTipo target) && !target.Elimindado)
                {
                   
                    target.Dispose();
                    this.NotificarInformacao($"Modulo {target.Nome}::{target.Id} removido com sucesso");
                    return Criar(moduloTipo.TipoModulo);
                }
                this.NotificarAviso($"Modulo {id} nao foi encontrado no provedor");
            }

            throw new ModuloNaoEncontradoException(Constantes.ERRO_MODULO_NEW_REINICIAR);

        }


        ///<inheritdoc/>
        public async Task ManterVivoAsync()
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
        public IEnumerable<IModulo> Listar()
        {
            return modules.Select(x => x.Value.Modulo);
        }

        private void Register(IModulo modulo)
        {
            if (modulo.InstanciaUnica && Existe(modulo.GetType()))
            {
                throw new ModuloInstanciaUnicaException(Constantes.ERRO_MODULO_INSTANCIA_UNICA);
            }

          
            if (!modules.ContainsKey(modulo.Id))
            {
                modules.TryAdd(modulo.Id, new ModuloTipo(modulo));
                this.NotificarInformacao($"Modulo {modulo.Id} registrado");
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
            Atual.Dispose();
        }

        ///<inheritdoc/>
        private static Type ResolverContrato(Type modulo)
        {
            if (!modulo.IsInterface && !modulo.IsClass)
            {
                throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_INVALIDO);
            }

            if (modulo.IsInterface)
            {
                ModuloContratoAttribute attr = modulo.ObterModuloContratoAtributo();

                if (attr is null)
                {
                    throw new ModuloContratoNaoEncontratoException(Constantes.ERRO_TIPO_NAO_MARCADO);
                }

                modulo = attr.Tipo;

                if (modulo is null)
                {
                    throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, attr.Nome));
                }

            }

            if (modulo.IsClass)
            {
                if (!modulo.IsAssignableTo(typeof(IModulo)))
                {
                    throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_NAO_HERDADO);
                }

                if (modulo.ObterModuloAtributo() is null)
                {
                    throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_NAO_MARCADO);
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
                TaskJob.GetTasker().UnregisterJob($"{this.GetType().FullName}::Unload_Modules");

            }
            base.Dispose(disposing);

        }
        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());
            _ = stringBuilder.Append("Ultima atualização: ").Append(UltimaAtualizacao).AppendLine();
            _ = stringBuilder.Append("Modulos inicializados: ").Append(ModulosInicializados).AppendLine();
            return stringBuilder.ToString();

        }


    }
}
