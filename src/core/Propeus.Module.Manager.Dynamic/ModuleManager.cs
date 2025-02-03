using System.Reflection;
using System.Text;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.IL.Core.Geradores;
using Propeus.Module.IL.Core.Helpers;
using Propeus.Module.MessageQueue.Contracts;
using Propeus.Module.Utils.Atributos;
using Propeus.Module.WatcherDynamicModule.Contracts;

namespace Propeus.Module.Manager.Dynamic
{



    /// <summary>
    /// ModuleProxy responsável por administrar modulos em tempo de execução
    /// </summary>
    [Module]
    public class ModuleManager : BaseModule, IModuleManager
    {

        /// <summary>
        /// Inicializa o gerenciador
        /// </summary>
        /// <param name="moduleManager">Gerenciador que irá controlar o modulo</param>
        /// <param name="messageQueueManagerContract"></param>
        ///<example>
        ///Criar uma instancia do gerenciador dinamico
        ///<code>
        ///using System;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManager(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///         {
        ///            //Seu codigo aqui...
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///</example>
        public ModuleManager(IModuleManager moduleManager, IMessageQueueManagerContract messageQueueManagerContract) : base()
        {
            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            StartDate = DateTime.Now;

            messageQueueManagerContract.RegisterOnQueue(Constantes.MENSAGERIA_LOAD_MODULE, (object args) => { ModuleManager_OnLoadModule(args as Type); });
            messageQueueManagerContract.RegisterOnQueue(Constantes.MENSAGERIA_RELOAD_MODULE, (object args) => { ModuleManager_OnReloadModule(args as Type); });
            messageQueueManagerContract.RegisterOnQueue(Constantes.MENSAGERIA_UNLOAD_MODULE, (object args) => { ModuleManager_OnUnloadModule(args as Type); });

            _gerenciador = moduleManager;
            _messageQueueManagerContract = messageQueueManagerContract;
        }


        private readonly IModuleManager _gerenciador;
        private readonly IMessageQueueManagerContract _messageQueueManagerContract;

        /// <summary>
        /// Diretório atual do modulo
        /// </summary>
        public string ModuleDirectory { get; set; } = Directory.GetCurrentDirectory();
        ///<inheritdoc/>
        public DateTime StartDate { get; }
        ///<inheritdoc/>
        public DateTime LastUpdate { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int InitializedModules => _gerenciador.InitializedModules;

        private Dictionary<string, ILClasseProvider> ModuloProvider { get; set; }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao herdado de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType nao encontrado pelo nome no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType ausente no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleSingleInstanceException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuleBuilderAbsentException">ObjectBuilder ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManagerArguments gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule&gt;ICalculadoraModuloContrato&lt;(new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public T CreateModule<T>(object[]? args = null) where T : IModule
        {
            T modulo = (T)CreateModule(typeof(T), args);
            return modulo;
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao herdado de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType nao encontrado pelo nome no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType ausente no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleSingleInstanceException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuleBuilderAbsentException">ObjectBuilder ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManagerArguments gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule(typeof(ICalculadoraModuloContrato),new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        ///</example>
        public IModule CreateModule(Type moduleType, object[]? args = null)
        {
            Type? contract = null;
            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            if (moduleType.IsInterface)
            {
                contract = moduleType;
                moduleType = ResolveContract(moduleType);
            }


            ConstructorInfo? ctor = moduleType
                .GetConstructors()
                .MaxBy(cto => cto.GetParameters().Length) ?? throw new ModuleBuilderAbsentException(moduleType);

            ParameterInfo[] @params = ctor.GetParameters();
            for (int i = 0; i < @params.Length; i++)
            {
                ParameterInfo @param = @params[i];
                //Esta validação é para casos de contrato, caso o parametro seja um modulo em si, o gerenciador base irá resolver
                if (param.ParameterType.IsInterface && param.ParameterType.PossuiAtributo<ModuleContractAttribute>())
                {
                    try
                    {
                        _ = CreateModule(param.ParameterType);
                    }
                    catch (ModuleNotFoundException)
                    {
                        if (!param.IsOptional)
                        {
                            throw;
                        }
                        //Ignora erro neste caso
                    }
                    catch (ModuleContractInvalidException)
                    {
                        /**
                         * Se o parametro for opcional, retorna valor default se houver, senao null
                         * So é obrigatorio valor padrão se o tipo for obrigatorio mas com valor padrão determinado
                         * Regras:
                         * 1 - Se for opcional, permitir nulo
                         * 2 - Se for obrigatório, deve possuir valor padrão (caso nao tenha sido preenchido)
                         * 
                         * Obs.: Dado o escopo do comando if, o tipo do parametro sempre sera interface
                         **/

                        if (!param.IsOptional)
                        {
                            throw;
                        }

                        if (!param.HasDefaultValue)
                        {
                            throw;
                        }
                        //Ignora erro neste caso
                    }
                    catch (ModuleSingleInstanceException)
                    {
                        //Caso já exista um modulo de instancia unica em execução E a interface de contrato nao estiver injetado nele,cria um modulo proxy
                        var moduleInstance = GetModule(param.ParameterType);
                        var typeModuleProxy = CreateProxyModuleSingleton(moduleInstance.GetType(), new Type[] { param.ParameterType });
                        var auxModule = CreateModule(typeModuleProxy);
                        if (args != null)
                        {
                            if (@params.Length > args.Length)
                            {
                                args = Utils.Objetos.Helper.JoinParameterValue(ctor, args);
                            }

                            args[i] = auxModule;
                        }
                        else
                        {
                            args = new object[] { auxModule };
                        }

                    }

                }

            }

            IModule? module = null;
            try
            {
                if (args != null && @params.Length >= args.Length)
                {
                    module = _gerenciador.CreateModule(moduleType, Utils.Objetos.Helper.JoinParameterValue(ctor, args));
                }
                else
                {
                    module = _gerenciador.CreateModule(moduleType);
                }
            }
            catch (ModuleSingleInstanceException)
            {
                if (contract is not null)
                {
                    // Se o usuario informar uma interface proxy, será realizado um proxy temporario
                    var moduleInstance = GetModule(moduleType);
                    var typeModuleProxy = CreateProxyModuleSingleton(moduleInstance.GetType(), new Type[] { contract });
                    module = _gerenciador.CreateModule(typeModuleProxy, args);
                }
                else
                {
                    throw;
                }

            }


            return module;
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao herdado de <see cref="IModule"/></exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo nao possui o atributo <see cref="ModuleAttribute"/></exception>
        ///<exception cref="ModuleTypeInvalidException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType nao encontrado pelo nome no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleTypeNotFoundException">ModuleType ausente no atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleSingleInstanceException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuleBuilderAbsentException">ObjectBuilder ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManagerArguments gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        /// <note type="important">
        /// Ao contrário do método do Gerenciador Core, este método consegue moduleWatcher interface de contrato pelo nome.
        /// </note>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        /// <note type="warning">
        /// Tome cuidado ao escrever o nome do module, pois este método é case-sensitive, ou seja, letra maiúscula e minúscula faz diferença.
        /// </note>
        ///</example>
        public IModule CreateModule(string moduleName, object[]? args = null)
        {
            var moduleWatcher = GetModule<IModuleWatcherContract>();
            return CreateModule(moduleWatcher[moduleName], args);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois remova
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///           using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.RemoveModule(modulo);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public void RemoveModule<T>(T moduleInstance) where T : IModule
        {
            _gerenciador.RemoveModule(moduleInstance);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuleDisposedException">Instancia do modulo foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois remova pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.RemoveModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public void RemoveModule(string idModule)
        {
            _gerenciador.RemoveModule(idModule);
        }



        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [ModuleProxy]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois recicle
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.RecycleModule(modulo);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///<note type="important">
        /// Garanta que o modulo a qual deseja reciclar esteja ativo, ou seja, seu status deve ser diferente de ,<see cref="State.Off"/> ou <see cref="State.Error"/> e não pode ter sido coletado e nem descartado
        ///</note>
        ///</example>
        public T RecycleModule<T>(T moduleInstance) where T : IModule
        {
            T nModulo = _gerenciador.RecycleModule(moduleInstance);
            return nModulo;
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuleDisposedException">Instancia do modulo foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [ModuleProxy]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois recicle pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo = gerenciador.RecycleModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///<note type="important">
        /// Garanta que o modulo a qual deseja reciclar esteja ativo, ou seja, seu status deve ser diferente de ,<see cref="State.Off"/> ou <see cref="State.Error"/> e não pode ter sido coletado e nem descartado
        ///</note>
        ///</example>
        public IModule RecycleModule(string idModule)
        {
            return _gerenciador.RecycleModule(idModule);
        }


        ///<inheritdoc/>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.GetModule&gt;ICalculadoraModuloContrato&lt;();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public T GetModule<T>() where T : IModule
        {
            return (T)GetModule(typeof(T));
        }
        ///<inheritdoc/>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo = gerenciador.GetModule(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        ///</example>
        public IModule GetModule(Type moduleType)
        {
            return _gerenciador.GetModule(ResolveContract(moduleType));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parâmetro nulo</exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.GetModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        /// <note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        ///</example>
        public IModule GetModule(string idModule)
        {
            return _gerenciador.GetModule(idModule);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                if(gerenciador.ExistsModule(typeof(ICalculadoraModuloContrato)))
        ///                {
        ///                     System.Console.WriteLine("Yay!!!");
        ///                }
        ///                else
        ///                {
        ///                     System.Console.WriteLine("Ops :(");
        ///                }
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public bool ExistsModule(Type moduleType)
        {

            if (ModuloProvider.TryGetValue(moduleType.Name, out ILClasseProvider? target))
            {
                return _gerenciador.ExistsModule(target.ObterTipoGerado());
            }
            else
            {
                return _gerenciador.ExistsModule(moduleType);
            }

        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                if(gerenciador.ExistsModule(modulo))
        ///                {
        ///                     System.Console.WriteLine("Yay!!!");
        ///                }
        ///                else
        ///                {
        ///                     System.Console.WriteLine("Ops :(");
        ///                }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public bool ExistsModule(IModule moduleInstance)
        {
            return _gerenciador.ExistsModule(moduleInstance);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                if(gerenciador.ExistsModule(modulo.Id))
        ///                {
        ///                     System.Console.WriteLine("Yay!!!");
        ///                }
        ///                else
        ///                {
        ///                     System.Console.WriteLine("Ops :(");
        ///                }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public bool ExistsModule(string idModule)
        {
            return _gerenciador.ExistsModule(idModule);
        }

        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                IEnumerable&gt;IModule&lt; =  gerenciador.ListAllModules();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public IEnumerable<IModule> ListAllModules()
        {
            return _gerenciador.ListAllModules();
        }

        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///    [Module]
        ///    public class CalculadoraModulo : BaseModule
        ///    {
        ///        public ModuloTesteA(IModuleManager _gerenciador) : base(_gerenciador, false)
        ///        {
        ///            
        ///        }
        ///        
        ///        public int Calcular(int a, int b)
        ///        {
        ///            return a+b;
        ///        }
        ///        
        ///        public void CriarInstancia(int valorTipoQualquer, string valorTipoString)
        ///        {
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Module.Manager.Dynamic;
        ///
        ///namespace Propeus.Module.Example
        ///{
        ///
        ///    [ModuleContract("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModule
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using (IModuleManager gerenciador = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.KeepAliveModuleAsync(modulo).Wait();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///</example>
        public void KeepAliveModule(IModule moduleInstance)
        {
            _gerenciador.KeepAliveModule(moduleInstance);
        }
        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {

            if (disposing && State != State.Off)
            {
                State = State.Off;
                //RemoveAllModules(); usar um registry para controlar os modulos do dinamico

                foreach (KeyValuePair<string, ILClasseProvider> item in ModuloProvider)
                {
                    item.Value.Dispose();
                }
                ModuloProvider.Clear();
                _messageQueueManagerContract.UnregisterOnQueue(Constantes.MENSAGERIA_LOAD_MODULE, (object args) => { ModuleManager_OnLoadModule(args as Type); });
                _messageQueueManagerContract.UnregisterOnQueue(Constantes.MENSAGERIA_RELOAD_MODULE, (object args) => { ModuleManager_OnReloadModule(args as Type); });
                _messageQueueManagerContract.UnregisterOnQueue(Constantes.MENSAGERIA_UNLOAD_MODULE, (object args) => { ModuleManager_OnUnloadModule(args as Type); });

                //ModuloProvider = null;
                GeradorHelper.DisposeGerador();


            }
            base.Dispose(disposing);
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());

            _ = stringBuilder.Append("Data de inicializacao: ").Append(StartDate).AppendLine();
            _ = stringBuilder.Append("Tempo em execução: ").Append(DateTime.Now - StartDate).AppendLine();

            _ = stringBuilder.Append("Caminho do diretório: ").Append(ModuleDirectory).AppendLine();
            //_ = stringBuilder.Append("Quantidade de DLLs no diretório: ").Append(ModuleProvider.Provider.ModulosDllCarregados).AppendLine();
            _ = stringBuilder.Append("Quantidade de caminho_modulos inicializados: ").Append(InitializedModules).AppendLine();

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Obtém o tipo implementado com base na interface informada
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns>ModuleType implementado</returns>
        /// <exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        /// <exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        private Type ResolveContract(Type contractType)
        {
            if (!contractType.IsInterface)
            {
                return contractType;
            }
            else
            {
                var moduleWatcher = _gerenciador.GetModule<IModuleWatcherContract>();
                if (moduleWatcher.HasModuleTypeFromContract(contractType))
                {
                    return moduleWatcher.GetModuleTypeFromContract(contractType);
                }
                else
                {
                    return moduleWatcher.BuildModuleTypeFromContract(contractType);
                }
            }

        }


        internal void ModuleManager_OnReloadModule(Type type)
        {
            ModuleAttribute? attr = type.GetModuleAttribute();
            if (attr.AutoUpdate)
            {
                IModule? module = null;
                if (attr.Singleton)
                {
                    if (ExistsModule(type))
                    {
                        RemoveModule(GetModule(type));
                        module = CreateModule(type);
                        if (attr.KeepAlive)
                        {
                            KeepAliveModule(module);
                        }
                    }
                    else
                    {
                        module = CreateModule(type);
                        if (attr.KeepAlive)
                        {
                            KeepAliveModule(module);
                        }
                    }


                }
                else
                {
                    var modules = ListAllModules().Where(x => x.GetType() == type).ToList();
                    foreach (var item in modules)
                    {
                        RemoveModule(item);

                    }

                    module = CreateModule(type);
                    if (attr.KeepAlive)
                    {
                        KeepAliveModule(module);
                    }
                }
            }
        }

        internal void ModuleManager_OnLoadModule(Type obj)
        {
            ModuleAttribute? attr = obj.GetModuleAttribute();
            if (attr.AutoStartable && (!ExistsModule(obj) || (!attr.Singleton && ExistsModule(obj))))
            {
                IModule? module = CreateModule(obj);
                if (attr.KeepAlive)
                {
                    KeepAliveModule(module);
                }

            }
        }

        internal void ModuleManager_OnUnloadModule(Type type)
        {
            ModuleAttribute? attr = type.GetModuleAttribute();
            if (attr.Singleton)
            {
                if (ExistsModule(type))
                {
                    RemoveModule(GetModule(type));
                }
            }
            else
            {
                var modules = ListAllModules().Where(x => x.GetType() == type).ToList();
                foreach (var item in modules)
                {
                    RemoveModule(item);
                }
            }
        }

        private Type CreateProxyModuleSingleton(Type type, Type[]? contracts = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.PossuiAtributo<ModuleContractAttribute>() && type.IsInterface && type.IsAssignableTo(typeof(IModule)))
            {
                var moduleType = ResolveContract(type);
                //Existe intancia de um modulo de instancia unica
                if (ExistsModule(moduleType))
                {
                    var moduleInstance = GetModule(moduleType);
                    //Ja possui a assinatura do contrato
                    if (moduleInstance.GetType().IsAssignableTo(type))
                    {
                        return moduleType;
                    }
                    //Vai ter que ser feito a busca de forma manual, pois os modulos deixam essa parte sem acesso
                    //Neste caso aqui é contruido um novo proxy e no construtor dele é passado o proxy original.
                    Propeus.Module.IL.Core.Helpers.GeradorHelper.GetCurrentInstanceOrNew(out IL.Core.Geradores.ILGerador iLGerador);
                    var IlModule = iLGerador.Modulo;
                    var IlModuleProvider = IlModule.CriarClasseTemporaria(moduleType, new Type[] { type });
                    IlModuleProvider.Apply();
                    //Verificar se o gerenciador consegue obter o proxy original e injetar no novo, caso contrario tera que criar a instancia aqui
                    return IlModuleProvider.ObterTipoGerado();

                }
                else
                {
                    return moduleType;
                }

            }
            else if (type.PossuiAtributo<ModuleAttribute>() && contracts is not null && contracts.All(c => c.IsInterface && c.PossuiAtributo<ModuleContractAttribute>() && c.IsAssignableTo(typeof(IModule))))
            {
                Propeus.Module.IL.Core.Helpers.GeradorHelper.GetCurrentInstanceOrNew(out IL.Core.Geradores.ILGerador iLGerador);
                var IlModule = iLGerador.Modulo;
                var IlModuleProvider = IlModule.CriarClasseTemporaria(type, contracts);
                IlModuleProvider.Apply();
                //Verificar se o gerenciador consegue obter o proxy original e injetar no novo, caso contrario tera que criar a instancia aqui
                return IlModuleProvider.ObterTipoGerado();

            }

            //Tipo invalido, nao pode passar o modulo implementado, ainda nao constuir uma classe para obter os mesmos resultados de uma bola de cristal.
            //Ou seja, não da para saber que contrato ele quer realziar o proxy se nao me informar.
            throw new NotImplementedException();
        }
    }
}
