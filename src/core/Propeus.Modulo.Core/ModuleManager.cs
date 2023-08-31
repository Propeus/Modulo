using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Tipos;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de módulos
    /// </summary>
    /// <example>
    /// Para iniciar este gerenciador deve utilizar o método de extensão chamado <see cref="ModuleManagerExtensions.CreateModuleManager"/>
    /// <code>
    ///  using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
    ///  {
    ///     //Seu código aqui...
    ///  }
    /// </code>
    /// </example>
    public class ModuleManager : BaseModel, IModuleManager
    {

        ///<inheritdoc/>
        public DateTime LastUpdate { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int InitializedModules { get; private set; }


        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        internal ModuleManager()
        {
            StartDate = DateTime.Now;
        }

        private readonly CancellationTokenSource _cancellationToken = new();

        //K:Id | V:moduleInstance
        private readonly ConcurrentDictionary<string, IModuleType> modules = new ConcurrentDictionary<string, IModuleType>();

        ///<inheritdoc/>
        ///<value>Data e hora que o gerenciador iniciou</value>
        public DateTime StartDate { get; private set; }

        ///<inheritdoc/>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        /// <note type="tip">
        /// Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        /// </note>
        /// 
        ///Para criar múltiplas instancias de um mesmo module caso o <see cref="IModule.IsSingleInstance"/> seja <see langword="false"/>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IInterfaceDeContratoDeExemplo module_a = gerenciador.CreateModule&gt;ModuloDeExemplo&lt;();
        ///              IInterfaceDeContratoDeExemplo module_b = gerenciador.CreateModule&gt;ModuloDeExemplo&lt;();
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///Para criar múltiplas instancias de um mesmo module por meio de uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule&gt;IInterfaceDeContratoDeExemplo&lt;();
        ///              IModule module_b = gerenciador.CreateModule&gt;IInterfaceDeContratoDeExemplo&lt;();
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// 
        /// </example>
        public T CreateModule<T>() where T : IModule
        {
            return (T)CreateModule(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O module não foi encontrado pelo nome informado</exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///}
        /// </code>
        /// 
        /// 
        ///Para criar múltiplas instancias de um mesmo module caso o <see cref="IModule.IsSingleInstance"/> seja <see langword="false"/>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule("ModuloDeExemplo");
        ///              IModule module_b = gerenciador.CreateModule("ModuloDeExemplo");
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// <note type="important">
        /// Este método não consegue resolver interface de contrato pelo nome, somente módulos.
        /// </note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// <note type="warning">
        /// Tome cuidado ao escrever o nome do module, pois este método é case-sensitive, ou seja, letra maiúscula e minúscula faz diferença.
        /// </note>
        /// </example>
        public IModule CreateModule(string moduleName)
        {
            Type result = null;
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();
            foreach (Assembly item in assemblies)
            {
                result = Array.Find(item.GetTypes(), x => x.Name == moduleName);
                if (result != null)
                {
                    break;
                }
            }

            return result == null
                ? throw new ModuleTypeNotFoundException(moduleName)
                : CreateModule(result);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O o parâmetro <paramref name="moduleType"/> é nulo</exception>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        /// <note type="tip">
        /// Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        /// </note>
        /// 
        ///Para criar múltiplas instancias de um mesmo module caso o <see cref="IModule.IsSingleInstance"/> seja <see langword="false"/>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IInterfaceDeContratoDeExemplo module_a = gerenciador.CreateModule(typeof(ModuloDeExemplo));
        ///              IInterfaceDeContratoDeExemplo module_b = gerenciador.CreateModule(typeof(ModuloDeExemplo));
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///Para criar múltiplas instancias de um mesmo module por meio de uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IInterfaceDeContratoDeExemplo module_a = gerenciador.CreateModule(typeof(IInterfaceDeContratoDeExemplo));
        ///              IInterfaceDeContratoDeExemplo module_b = gerenciador.CreateModule(typeof(IInterfaceDeContratoDeExemplo));
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// </example>
        public IModule CreateModule(Type moduleType)
        {
            CheckModuleManagerStatus();

            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }
            moduleType = ResolveContract(moduleType);

            if (moduleType.GetModuleAttribute().Singleton && ExistsModule(moduleType))
            {
                throw new ModuleSingleInstanceException(moduleType);
            }

            ConstructorInfo ctor = moduleType.GetConstructors().MaxBy(x => x.GetParameters().Length);
            if (ctor is null)
            {
                throw new ModuleBuilderAbsentException(moduleType);
            }

            ParameterInfo[] paramCtor = ctor.GetParameters();
            object[] args = new object[paramCtor.Length];

            for (int i = 0; i < paramCtor.Length; i++)
            {
                if (paramCtor[i].ParameterType.IsAssignableTo(typeof(IModuleManager)))
                {
                    IModuleType gen = modules
                        .Where(x => !x.Value.IsDeleted)
                        .Select(x => x.Value)
                        .FirstOrDefault(x => x.Module is IModuleManager);
                    args[i] = gen?.Module as IModuleManager ?? this;
                }
                else if (paramCtor[i].ParameterType.IsAssignableTo(typeof(IModule)) && paramCtor[i].ParameterType.PossuiAtributo<ModuleContractAttribute>())
                {
                    try
                    {
                        if (ExistsModule(paramCtor[i].ParameterType))
                        {
                            var module = GetModule(paramCtor[i].ParameterType);
                            if (module.GetType().GetModuleAttribute().Singleton)
                            {
                                args[i] = module;
                            }
                        }

                        if (args[i] is null)
                        {
                            args[i] = CreateModule(paramCtor[i].ParameterType);  
                        }

                        var attr = args[i].GetType().GetModuleAttribute();
                        if (attr.Singleton && attr.KeepAlive || attr.AutoStartable && attr.Singleton)
                        {
                            KeepAliveModule(args[i] as IModule);
                        }
                    }
                    catch (ModuleTypeNotFoundException)
                    {
                        if (paramCtor[i].IsOptional)
                        {
                            args[i] = paramCtor[i].ParameterType.Default();
                            continue;
                        }

                        throw;
                    }
                }


                if (paramCtor[i].HasDefaultValue || paramCtor[i].IsOptional || paramCtor[i].IsNullable())
                {
                    if (!(paramCtor[i].DefaultValue is DBNull))
                        args[i] = paramCtor[i].DefaultValue;
                    continue;
                }

                throw new ModuleTypeInvalidException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Module, Contrato ou ModuleManager");
            }



            IModule modulo = (IModule)Activator.CreateInstance(moduleType, args);
            Register(modulo);
            InitializedModules++;
            return modulo;

        }



        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O parâmetro é nulo</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se existe alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule(typeof(ModuloDeExemplo));
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 System.Console.WriteLine("Existe o module! Obaaa!");
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// </example>
        public bool ExistsModule(Type moduleType)
        {
            CheckModuleManagerStatus();

            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            try
            {
                moduleType = ResolveContract(moduleType);
                IModuleType moduloInstancia = modules.Values.FirstOrDefault(x => x.Name == moduleType.Name);

                return moduloInstancia is not null && !moduloInstancia.IsDeleted && !moduloInstancia.IsCollected;
            }
            catch (ModuleTypeNotFoundException)
            {
                return false;
            }

        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O parâmetro é nulo</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se a instancia está registrado no gerenciador
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;());
        ///              if(gerenciador.ExistsModule(module_a)
        ///              {
        ///                 System.Console.WriteLine("Existe o module! Obaaa!");
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public bool ExistsModule(IModule moduleInstance)
        {
            return moduleInstance is null ? throw new ArgumentNullException(nameof(moduleInstance)) : ExistsModule(moduleInstance.Id);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O parâmetro é nulo</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se existe o module pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;());
        ///              if(gerenciador.ExistsModule(module_a.Id)
        ///              {
        ///                 System.Console.WriteLine("Existe o module! Obaaa!");
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public bool ExistsModule(string idModule)
        {
            CheckModuleManagerStatus();

            if (string.IsNullOrEmpty(idModule))
            {
                throw new ArgumentNullException(nameof(idModule));
            }


            return modules.TryGetValue(idModule, out IModuleType moduleType) && !moduleType.IsCollected && !moduleType.IsDeleted;

        }

        ///<inheritdoc/>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              _ = gerenciador.CreateModule(typeof(ModuloDeExemplo));
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 IModule module_a = gerenciador.GetModule(typeof(ModuloDeExemplo))
        ///                 System.Console.WriteLine(module_a);
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// </example>
        public IModule GetModule(Type moduleType)
        {
            CheckModuleManagerStatus();

            moduleType = ResolveContract(moduleType);

            IModuleType moduloInstancia = modules.Values.FirstOrDefault(x => x.Name == moduleType.Name && !x.IsDeleted && !x.IsCollected) ?? throw new ModuleNotFoundException(moduleType);


            return moduloInstancia.Module;
        }
        ///<inheritdoc/>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              _ = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 ModuloDeExemplo module_a = gerenciador.GetModule&lt;ModuloDeExemplo&gt;();
        ///                 System.Console.WriteLine(module_a);
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public T GetModule<T>() where T : IModule
        {
            return (T)GetModule(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentException">Parâmetro nulo ou vazio</exception>
        ///<exception cref="ModuleDisposedException">O module informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O module informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter instancia pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              ModuloDeExemplo module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 IModule module_b = gerenciador.GetModule(module_a.Id);
        ///                 System.Console.WriteLine(module_b);
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public IModule GetModule(string idModule)
        {
            CheckModuleManagerStatus();

            if (string.IsNullOrEmpty(idModule))
            {
                throw new ArgumentException($"'{nameof(idModule)}' não pode ser nulo nem vazio.", nameof(idModule));
            }


            if (modules.TryGetValue(idModule, out IModuleType moduloInstancia))
            {
                if (moduloInstancia.IsDeleted || moduloInstancia.IsCollected)
                {
                    throw new ModuleDisposedException(moduloInstancia.IdModule);
                }

                return moduloInstancia.Module;
            }

            throw new ModuleNotFoundException(idModule);


        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parâmetro nulo</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para remover algum contractType pela instancia
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              ModuloDeExemplo module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 gerenciador.RemoveModule(module_a);
        ///                 System.Console.WriteLine(module_a);
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public void RemoveModule<T>(T moduleInstance) where T : IModule
        {
            if (moduleInstance is null)
            {
                throw new ArgumentNullException(nameof(moduleInstance));
            }

            RemoveModule(moduleInstance.Id);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentException">Parâmetro nulo ou vazio</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para remover algum contractType pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              ModuloDeExemplo module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              if(gerenciador.ExistsModule(typeof(IInterfaceDeContratoDeExemplo))
        ///              {
        ///                 gerenciador.RemoveModule(module_a.Id);
        ///                 System.Console.WriteLine(module_a);
        ///              }else{
        ///                 System.Console.WriteLine("Algo de errado nao esta certo...");
        ///              }
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public void RemoveModule(string idModule)
        {
            if (string.IsNullOrEmpty(idModule))
            {
                throw new ArgumentException($"'{nameof(idModule)}' não pode ser nulo nem vazio.", nameof(idModule));
            }

            if (modules.TryRemove(idModule, out IModuleType target))
            {
                target.Dispose();
                InitializedModules--;
            }

        }
        ///<inheritdoc/>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para remover todos os módulos ativos
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              ModuloDeExemplo module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              gerenciador.RemoveAllModules();
        ///              System.Console.WriteLine(module_a);
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        /// </example>
        public void RemoveAllModules()
        {
            foreach (KeyValuePair<string, IModuleType> item in modules)
            {
                RemoveModule(item.Key);
            }

            modules.Clear();

        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parâmetro nulo</exception>
        ///<exception cref="ModuleDisposedException">O module informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O module informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para reciclar um contractType
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              ModuloDeExemplo module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              System.Console.WriteLine(module_a);
        ///              module_a = gerenciador.RecycleModule(module_a);
        ///              System.Console.WriteLine(module_a);
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///<note type="important">
        /// Garanta que o contractType a qual deseja reciclar esteja ativo, ou seja, seu status deve ser diferente de ,<see cref="State.Off"/> ou <see cref="State.Error"/> e não pode ter sido coletado e nem descartado
        ///</note>
        /// </example>
        public T RecycleModule<T>(T moduleInstance) where T : IModule
        {
            return (T)RecycleModule(moduleInstance.Id);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentException">Parâmetro nulo ou vazio</exception>
        ///<exception cref="ModuleDisposedException">O module informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O module informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte module e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///  [ModuleContract(typeof(ModuloDeExemplo))]
        ///  internal interface IInterfaceDeContratoDeExemplo : IModule
        ///  {
        ///      void EscreverOlaMundo();
        ///  }
        ///}
        /// </code>
        /// 
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para reciclar um contractType pelo id
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              System.Console.WriteLine(module_a);
        ///              module_a = gerenciador.RecycleModule(module_a.Id);
        ///              System.Console.WriteLine(module_a);
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///<note type="important">
        /// Garanta que o contractType a qual deseja reciclar esteja ativo, ou seja, seu status deve ser diferente de ,<see cref="State.Off"/> ou <see cref="State.Error"/> e não pode ter sido coletado e nem descartado
        ///</note>
        ///</example>
        public IModule RecycleModule(string idModule)
        {
            Type moduleType = GetModule(idModule).GetType();
            RemoveModule(idModule);
            return CreateModule(moduleType);
        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentException">Parâmetro nulo</exception>
        ///<exception cref="ModuleDisposedException">O module informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O module informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do module no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte contractType e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///}
        /// </code>
        /// 
        /// 
        ///Para manter vivo um contractType 
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              IModule module_a = gerenciador.CreateModule&lt;ModuloDeExemplo&gt;();
        ///              System.Console.WriteLine(module_a);
        ///              gerenciador.KeepAliveModule(module_a).Wait();
        ///              System.Console.WriteLine(module_a);
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///</example>
        public void KeepAliveModule(IModule moduleInstance)
        {
            if (moduleInstance is null)
            {
                throw new ArgumentNullException(nameof(moduleInstance));
            }

            _ = GetModule(moduleInstance.Id);

            if (modules.TryGetValue(moduleInstance.Id, out IModuleType moduloTipo))
            {
                moduloTipo.KeepAliveModule(true);
            }
            else
            {
                throw new ModuleNotFoundException(moduleInstance.Id);

            }
        }

        ///<inheritdoc/>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte contractType e sua interface de contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato;
        ///using Propeus.Modulo.Abstrato.Attributes;
        ///using Propeus.Modulo.Abstrato.Interfaces;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  [Module]
        ///  internal class ModuloDeExemplo : BaseModule, IInterfaceDeContratoDeExemplo
        ///  {
        ///
        ///      public ModuloDeExemploParaPropeusModuloCore() : base(false)
        ///      {
        ///      }
        ///
        ///      public void EscreverOlaMundo()
        ///      {
        ///          System.Console.WriteLine("Ola mundo!");
        ///          System.Console.WriteLine("Este é um module em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///}
        /// </code>
        /// 
        /// 
        ///Para listar todos os modulos
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Core;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              gerenciador.KeepAliveModule(gerenciador.CreateModule&lt;ModuloDeExemplo&gt;());
        ///              foreach(IModule module in gerenciador.ListAllModules())
        ///              {
        ///                 System.Console.WriteLine(module);
        ///              }    
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///</example>
        public IEnumerable<IModule> ListAllModules()
        {
            return modules.Select(x => x.Value.Module);
        }



        /// <summary>
        /// Obtém o tipo implementado com base na interface informada
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns>ModuleType implementado</returns>
        /// <exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        /// <exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        /// <exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        /// <exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        /// <exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        /// <exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        /// <exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        private Type ResolveContract(Type contractType)
        {
            if (contractType.IsInterface)
            {
                ModuleContractAttribute attr = contractType.GetAttributeContractModule() ?? throw new ModuleContractNotFoundException(contractType);
                contractType = attr.ModuleType;
                if (contractType is null)
                {
                    foreach (var item in modules.Where(item => item.Value.Name == attr.ModuleName))
                    {
                        contractType = item.Value.ModuleType;
                    }
                }

                if (contractType is null)
                {
                    throw new ModuleTypeNotFoundException(attr.ModuleName);
                }
            }

            if (contractType.IsClass)
            {
                if (!contractType.IsAssignableTo(typeof(IModule)))
                {
                    throw new ModuleTypeInvalidException.TypeModuleNotInheritedException(contractType);
                }

                if (contractType.GetModuleAttribute() is null)
                {
                    throw new ModuleTypeInvalidException.TypeModuleUnmarkedException(contractType);
                }

                return contractType;
            }

            throw new ModuleTypeInvalidException(Constantes.ERRO_TIPO_INVALIDO);

        }

        [Obsolete("Não tem mais servintia, será removido na proxima otimização",false)]
        /// <summary>
        /// Registra um module no gerenciador
        /// </summary>
        /// <param name="module">Instancia do module</param>
        /// <exception cref="ModuleSingleInstanceException">O module não pode ser registrado, pois já existe uma instancia em execução</exception>
        private void Register(IModule module)
        {
            modules.TryAdd(module.Id, new ModuloTipo(module));
        }


        /// <summary>
        /// Verifica se o gerenciador esta desligado ou liberado (disposed)
        /// </summary>
        /// <exception cref="ModuleManagerDisposedException">Quando o gerenciador chama o <see cref="IDisposable.Dispose()"/></exception>
        /// <exception cref="ModuleException">Quando o gerenciador esta desligado (<see cref="State.Off"/></exception>
        private void CheckModuleManagerStatus()
        {
            if (disposedValue)
            {
                throw new ModuleManagerDisposedException();
            }

            if (State == State.Off)
            {
                throw new ModuleException("O gerenciador se encontra desligado");
            }
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
