using System.Reflection;
using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Registry.Modules;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.Utils.Objetos;
using Propeus.Module.Utils.Tipos;

namespace Propeus.Module.Manager
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
        public int InitializedModules => Registry.InitializedModules;


        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        internal ModuleManager()
        {
            StartDate = DateTime.Now;
            Registry = new RegistryModule();
            Registry.RegisterModule(Registry);
        }

        private readonly CancellationTokenSource _cancellationToken = new();


        private RegistryModule Registry;


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
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        /// Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        /// </note>
        /// 
        ///Para criar múltiplas instancias de um mesmo modulo por meio de uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        public T CreateModule<T>(object[]? args = null) where T : IModule
        {
            return (T)CreateModule(typeof(T), args);
        }
        ///<inheritdoc/>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O modulo não foi encontrado pelo nome informado</exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
        ///      }
        ///
        ///  }
        ///
        ///}
        /// </code>
        /// 
        /// <note type="important">
        /// Este método não consegue resolver interface de contrato pelo nome, somente módulos.
        /// </note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// <note type="warning">
        /// Tome cuidado ao escrever o nome do modulo, pois este método é case-sensitive, ou seja, letra maiúscula e minúscula faz diferença.
        /// </note>
        /// </example>
        public IModule CreateModule(string moduleName, object[]? args = null)
        {
            Type? result = null;
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();
            foreach (Assembly item in assemblies)
            {
                result = Array.Find(item.GetTypes(), x => x.Name == moduleName);
                if (result != null)
                {
                    break;
                }
            }

            if (result is null)
            {
                throw new ModuleTypeNotFoundException(moduleName);
            }
            else
            {
                return CreateModule(result, args);
            }
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
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        /// Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        /// </note>
        /// 
        ///Para criar múltiplas instancias de um mesmo modulo caso o <see cref="ModuleAttribute.Singleton"/> seja <see langword="false"/>
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///Para criar múltiplas instancias de um mesmo modulo por meio de uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        public IModule CreateModule(Type moduleType, object[]? args = null)
        {
            CheckModuleManagerStatus();

            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }
            moduleType = ResolveContract(moduleType);
            ModuleAttribute? attrModule = moduleType.GetModuleAttribute();

            if (attrModule.Singleton && ExistsModule(moduleType))
            {
                throw new ModuleSingleInstanceException(moduleType);
            }
            ConstructorInfo[]? ctors = moduleType.GetConstructors();
            ConstructorInfo ctor = null;
            if (args != null)
            {
                if (ctors.Any(x => x.GetParameters().Length == args.Length))
                {
                    ctor = ctors.First(x => x.GetParameters().Length == args.Length);
                }
                else
                {
                    ctor = ctors.MaxBy(x => x.GetParameters().Length);
                }
            }
            else
            {
                ctor = ctors.MaxBy(x => x.GetParameters().Length);
            }
            if (ctor is null)
            {
                throw new ModuleBuilderAbsentException(moduleType);
            }

            ParameterInfo[] paramCtor = ctor.GetParameters();

            object[] nArgs = Utils.Objetos.Helper.JoinParameterValue(ctor, args, LoadModuleFromParameter);
            IModule modulo = (IModule)Activator.CreateInstance(moduleType, nArgs);
            modulo.ConfigureModule();
            modulo.Launch();
            Registry.RegisterModule(modulo);
            return modulo;
        }





        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O parâmetro é nulo</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se existe alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///                 System.Console.WriteLine("Existe o modulo! Obaaa!");
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
                IModuleInformation moduloInstancia = Registry.GetAllModulesInformation().FirstOrDefault(x => x.Name == moduleType.Name);

                return moduloInstancia is not null && !moduloInstancia.IsDeleted;
            }
            catch (ModuleTypeNotFoundException)
            {
                return false;
            }

        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">O parâmetro é nulo</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se a instancia está registrado no gerenciador
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///                 System.Console.WriteLine("Existe o modulo! Obaaa!");
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
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para verificar se existe o modulo pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///                 System.Console.WriteLine("Existe o modulo! Obaaa!");
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


            return Registry.ExistsModule(idModule) && !Registry.GetModuleInformation(idModule).IsDeleted;

        }

        ///<inheritdoc/>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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

            Type moduleTypeResolved = ResolveContract(moduleType);

            IEnumerable<IModuleInformation>? moduleQuery = Registry.GetAllModulesInformation();
            moduleQuery = moduleQuery.Where(module => module.Name == moduleTypeResolved.Name);
            moduleQuery = moduleQuery.Where(module => !module.IsDeleted);

            IModuleInformation? moduleInformation = moduleQuery.FirstOrDefault();

            if (moduleInformation is null)
            {
                /*
                 * Vai ter quebra de regra aqui.
                 * Normalmente o GetModule deve retornar uma instancia existente caso contrario é excepton
                 * Porem caso seja solicitado a instancia do objeto existente para uma nova interface de contrato, 
                 * devera ser criado um novo proxy de modulo encapsulado a instancia original, sendo assim mantendo a regra de obter uma instancia existente porem 
                 * com um proxy novo
                 */
                if (moduleTypeResolved.Name.Contains(Propeus.Module.Abstract.Constantes.STR_TYPE_TEMPORARY))
                {
                    return CreateModule(moduleTypeResolved);
                }

                throw new ModuleNotFoundException(moduleTypeResolved);
            }



            return moduleInformation.Module;
        }
        ///<inheritdoc/>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter alguma instancia do tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///<exception cref="ModuleDisposedException">O modulo informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O modulo informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para obter instancia pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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

            IModuleInformation? module = Registry.GetModuleInformation(idModule);

            if (module != null)
            {
                if (module.IsDeleted)
                {
                    throw new ModuleDisposedException(module.Module);
                }

                return module.Module;
            }

            throw new ModuleNotFoundException(idModule);


        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parâmetro nulo</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para remover algum contractType pela instancia
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para remover algum contractType pelo Id
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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

            Registry.UnregisterModule(idModule);

        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parâmetro nulo</exception>
        ///<exception cref="ModuleDisposedException">O modulo informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O modulo informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para reciclar um contractType
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///<exception cref="ModuleDisposedException">O modulo informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O modulo informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///
        ///<exception cref="ModuleBuilderAbsentException">Não há um construtor publico disponível</exception>
        ///<exception cref="ModuleContractNotFoundException">A interface não possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">O <see cref="ModuleContractAttribute"/> não possui um tipo ou nome valido</exception>
        ///<exception cref="ModuleTypeInvalidException">O tipo não é nem <see langword="class"/> e nem <see langword="interface"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não herda de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte modulo e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///Um modulo não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// 
        ///Para reciclar um contractType pelo id
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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
        ///<exception cref="ModuleDisposedException">O modulo informado foi coletado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleDisposedException">O modulo informado foi apagado pelo <see cref="GC"/></exception>
        ///<exception cref="ModuleNotFoundException">Não existe uma instancia valida do tipo do modulo no gerenciador</exception>
        ///<example>
        ///Para os exemplos abaixo será utilizado o seguinte contractType e sua interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
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

            CheckModuleManagerStatus();

            if (moduleInstance is null)
            {
                throw new ArgumentNullException(nameof(moduleInstance));
            }

            var moduleInfo = Registry.GetModuleInformation(moduleInstance.Id);
            if (moduleInfo != null)
            {
                if (moduleInfo.IsDeleted || moduleInfo.IsCollected)
                {
                    throw new ModuleDisposedException(moduleInstance.Id);
                }

                moduleInfo.KeepAliveModule(true);
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
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///
        ///namespace Propeus.Module.Example
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
        ///          System.Console.WriteLine("Este é um modulo em funcionamento!");
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
        ///using Propeus.Module.Manager;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = ModuleManagerExtensions.CreateModuleManager())
        ///         {
        ///              gerenciador.KeepAliveModule(gerenciador.CreateModule&lt;ModuloDeExemplo&gt;());
        ///              foreach(IModule modulo in gerenciador.ListAllModules())
        ///              {
        ///                 System.Console.WriteLine(modulo);
        ///              }    
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///</example>
        public IEnumerable<IModule> ListAllModules()
        {
            CheckModuleManagerStatus();

            return Registry.GetAllModulesInformation().Select(x => x.Module);
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
        /// <exception cref="ModuleTypeInvalidException.TypeModuleUnmarkedException">A classe informada não possui o atributo <see cref="ModuleAttribute"/></exception>
        private Type ResolveContract(Type contractType)
        {
            if (contractType.IsInterface)
            {
                Type? result = null;
                IEnumerable<IModuleInformation>? modulesInformation = null;
                /**
                 * Metodo de pesquisa
                 * 1 - Proprio atributo (O mais rapido)
                 * 2 - Registry (Pode variar, más é mais lento que o item 1)
                 * 3 - Runtime do .NET (O mais lento de todos e ultimo recurso)
                 */

                //Verifica se o atributo possui algum tipo definido
                ModuleContractAttribute attr = contractType.GetAttributeContractModule() ?? throw new ModuleContractNotFoundException(contractType);
                result = attr.ModuleType;

                //Verifica se no registry tem alguem com a interface implementado
                if (result is null)
                {
                    modulesInformation = Registry.GetAllModulesInformation();
                    Type interfaceContract = contractType;
                    result = modulesInformation.FirstOrDefault(item => (!item.IsDeleted && item.ModuleType.IsAssignableTo(interfaceContract)) || item.Name == attr.ModuleName)?.ModuleType;
                }

                //Verifica se no runtime do .NET tem algum modulo de mesmo nome
                if (result is null)
                {
                    var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                    var types = assemblys.SelectMany(asm => asm.GetTypes());
                    var typeModule = types.FirstOrDefault(t => t.Name == attr.ModuleName);
                    result = typeModule;
                }

                //Senao ja era
                if (result is null)
                {
                    throw new ModuleTypeNotFoundException(attr.ModuleName);
                }

                return result;

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
        }

        private object LoadModuleFromParameter(ParameterInfo parameterInfo)
        {
            //Verifica-se se o parametro do construtor é um gerenciador de modulo
            if (parameterInfo.ParameterType.IsAssignableTo(typeof(IModuleManager)))
            {
                //Caso seja, será obtido o gerenciador do topo da pilha, caso nao exista nenhum gerenciador ativo, será retornado este
                IModuleInformation gen = Registry.GetAllModulesInformation()
                    .Where(x => !x.IsDeleted)
                    .LastOrDefault(x => x.Module is IModuleManager);
                return gen?.Module as IModuleManager ?? this;
            }
            //Se o parametro for um modulo qualquer...
            else if (parameterInfo.ParameterType.IsAssignableTo(typeof(IModule)))
            {
                //E possuir o atributo de contrato ou atributo de modulo...
                if (parameterInfo.ParameterType.PossuiAtributo<ModuleContractAttribute>() || parameterInfo.ParameterType.PossuiAtributo<ModuleAttribute>())
                {
                    try
                    {
                        IModule moduleInstance = null;
                        //É verificado se existe algum modulo do tipo, em atividade.
                        if (ExistsModule(parameterInfo.ParameterType))
                        {
                            //É obtido a instancia do modulo
                            try
                            {
                                var module = GetModule(parameterInfo.ParameterType);
                                var moduleType = module.GetType();
                                if (moduleType.GetModuleAttribute().Singleton && moduleType.IsAssignableTo(parameterInfo.ParameterType))
                                {
                                    moduleInstance = module;
                                }
                            }
                            catch (ModuleNotFoundException)
                            {
                                /*
                                 * O G.C. pode coletar o modulo entre o intervalo de tempo entre as chamadas de função 'ExistsModule' e 'GetModule'.
                                 * Para este caso, esta exceção será ignorada, pois o 'CreateModule' irá ser acionado quando não houver nenhum modulo na variavel 'moduleInstance'
                                 */
                            }
                        }
                        //Caso o modulo não tenha sido encontrado, será criado um novo
                        moduleInstance ??= CreateModule(parameterInfo.ParameterType);

                        //Obtem o atributo "Module" da instancia atual
                        var moduleAttribute = moduleInstance.GetType().GetModuleAttribute();
                        /*
                         * Este modulo so pode ficar vivo quando:
                         * 1 - O modulo for instancia unica e com o atributo 'KeepAlive' como true, pois pode haver vazamento de memoria coma  criação de n modulos
                         * 2 - Se um modulo for instancia unica e auto inicializavel, implicitamente o modulo será mantido vivo, pois entende-se como um worker service .
                         */
                        if ((moduleAttribute.Singleton && moduleAttribute.KeepAlive) || (moduleAttribute.AutoStartable && moduleAttribute.Singleton))
                        {
                            KeepAliveModule(moduleInstance);
                        }

                        return moduleInstance;
                    }
                    catch (ModuleTypeNotFoundException)
                    {
                        //Caso o gerador de modulos não conssiga resolver o tipo e o parametro for opcional...
                        if (parameterInfo.IsOptional)
                        {
                            //Retorna o valor padão do parametro
                            return parameterInfo.ParameterType.Default();
                        }

                        //Caso contrario, lança a exceção para o nivel acima
                        throw;
                    }
                }
                //Senão se, o parametro possuir valor padrão OU for opcional OU for nulavel (?)...
                else if (parameterInfo.HasDefaultValue || parameterInfo.IsOptional || parameterInfo.IsNullable())
                {
                    //Se o parametro nao possuir o valor padrão oridinal...
                    if (parameterInfo.DefaultValue is not DBNull)
                    {
                        return parameterInfo.DefaultValue; //Retorna o valor padrão
                    }
                }

            }

            //Caso nenhuma condição satsfaça, retorna nulo
            return null;
        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposedValue)
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
