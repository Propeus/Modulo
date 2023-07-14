using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Dinamico.Modules;
using Propeus.Modulo.IL.Core.Geradores;
using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Dinamico
{


  
    /// <summary>
    /// ModuleProxy responsável por administrar modulos em tempo de execução
    /// </summary>
    [Module]
    public class ModuleManager : BaseModule, IModuleManagerArguments
    {

        /// <summary>
        /// Inicializa o _gerenciador
        /// </summary>
        /// <param name="gerenciador">ModuleManager que irá controlar o modulo</param>
        public ModuleManager(IModuleManager gerenciador) : base(true)
        {


            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            StartDate = DateTime.Now;

            _gerenciador = gerenciador;
        }


        private readonly IModuleManager _gerenciador;

        /// <summary>
        /// Diretório atual do modulo
        /// </summary>
        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule&gt;ICalculadoraModuloContrato&lt;();
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T CreateModule<T>() where T : IModule
        {
            return (T)CreateModule(typeof(T));
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule CreateModule(Type moduleType)
        {
            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            if (moduleType.IsInterface)
            {
                moduleType = ResoverContratos(moduleType);
            }

            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            ConstructorInfo ctor = moduleType.GetConstructors().MaxBy(cto => cto.GetParameters().Length);

            ParameterInfo[] @params = ctor.GetParameters();
            foreach (ParameterInfo @param in @params)
            {
                if (param.ParameterType.IsInterface && param.ParameterType.PossuiAtributo<ModuleContractAttribute>())
                {
                    if (param.IsOptional)
                    {
                        try
                        {
                            _ = CreateModule(param.ParameterType);
                        }
                        catch (ModuleNotFoundException)
                        {
                            //Ignora erro neste caso
                        }
                    }
                    else
                    {
                        _ = CreateModule(param.ParameterType);
                    }
                }

            }

            return _gerenciador.CreateModule(moduleType);


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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato");
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule CreateModule(string moduleName)
        {
            //TODO: Criar exception para caso do modulo ModuleProviderModule nao existir
            return CreateModule(this.GetModule<ModuleProviderModule>()[moduleName]);
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule&gt;ICalculadoraModuloContrato&lt;(new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T CreateModule<T>(object[] args) where T : IModule
        {
            T modulo = (T)CreateModule(typeof(T));
            InvocarInstanciaConfiguracao(args, modulo);
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule(typeof(ICalculadoraModuloContrato),new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule CreateModule(Type moduleType, object[] args)
        {
            IModule iModulo = CreateModule(moduleType);
            InvocarInstanciaConfiguracao(args, iModulo);
            return iModulo;
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule CreateModule(string moduleName, object[] args)
        {
            IModule iModulo = CreateModule(moduleName);
            InvocarInstanciaConfiguracao(args, iModulo);

            return iModulo;
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois remova
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                _gerenciador.RemoveModule(modulo);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois remova pelo ID
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                _gerenciador.RemoveModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void RemoveModule(string idModule)
        {
            _gerenciador.RemoveModule(idModule);
        }
        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois remova pelo ID
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                _gerenciador.RemoveAllModules();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void RemoveAllModules()
        {
            _gerenciador.RemoveAllModules();
        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                _gerenciador.Recilcar(modulo);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.Recilcar(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule RecycleModule(string idModule)
        {
            return _gerenciador.RecycleModule(idModule);
        }


        ///<inheritdoc/>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.GetModule&gt;ICalculadoraModuloContrato&lt;();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.GetModule(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule GetModule(Type moduleType)
        {
            return _gerenciador.GetModule(moduleType);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleNotFoundException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.GetModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModule GetModule(string moduleType)
        {
            return _gerenciador.GetModule(moduleType);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuleTypeInvalidException">ModuleType do modulo invalido</exception>
        ///<exception cref="ModuleContractNotFoundException">ModuleType da interface de contrato nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.ExistsModule(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool ExistsModule(Type moduleType)
        {

            if (ModuloProvider.TryGetValue(moduleType.Name, out var target))
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.ExistsModule(modulo);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia pelo ID
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  _gerenciador.ExistsModule(modulo.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                IEnumerable&gt;IModule&lt; =  _gerenciador.ListAllModules();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
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
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///No projeto principal, adicione uma interface de contrato e depois mantenha vivo a instancia do _gerenciador
        ///<code>
        ///using System;
        ///using Propeus.ModuleProxy.Abstrato.Atributos;
        ///using Propeus.ModuleProxy.Core.ModuleManager;
        ///using Propeus.ModuleProxy.Dinamico.ModuleManager;
        ///
        ///namespace Propeus.ModuleProxy.Exemplo
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
        ///            using(ModuleManager _gerenciador = new ModuleManager(Propeus.ModuleProxy.Core.ModuleManager.Atual))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)_gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                _gerenciador.KeepAliveAsync().Wait();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public async Task KeepAliveAsync()
        {
            await _gerenciador.KeepAliveAsync().ConfigureAwait(true);
        }
        ///<inheritdoc/>
        public async Task KeepAliveModuleAsync(IModule moduleInstance)
        {
            await _gerenciador.KeepAliveModuleAsync(moduleInstance).ConfigureAwait(true);
        }
        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {

            if (disposing && State != State.Off)
            {
                State = State.Off;
                RemoveAllModules();

                foreach (var item in this.ModuloProvider)
                {
                    item.Value.Dispose();
                }
                ModuloProvider.Clear();
                ModuloProvider = null;
                GeradorHelper.Gerador.Dispose();

            }
            base.Dispose(disposing);
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());

            _ = stringBuilder.Append("Data de inicializacao: ").Append(StartDate).AppendLine();
            _ = stringBuilder.Append("Tempo em execução: ").Append(DateTime.Now - StartDate).AppendLine();

            _ = stringBuilder.Append("Caminho do diretório: ").Append(DiretorioModulo).AppendLine();
            //_ = stringBuilder.Append("Quantidade de DLLs no diretório: ").Append(ModuleProvider.Provider.ModulosDllCarregados).AppendLine();
            _ = stringBuilder.Append("Quantidade de caminho_modulos inicializados: ").Append(InitializedModules).AppendLine();

            return stringBuilder.ToString();
        }

        //TODO: Mover o ResoverContratos para o moduleProvider

        /// <summary>
        /// Obtem o tipo implementado com base na interface informada
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns>ModuleType implementado</returns>
        /// <exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        /// <exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuleContractAttribute"/></exception>
        private Type ResoverContratos(Type contrato)
        {
            if (!contrato.IsInterface)
            {
                throw new ArgumentException("O tipo nao e uma interface");
            }

            return this._gerenciador.GetModule<ModuleProviderModule>().GetModuleFromContract(contrato);

        }
        private static void InvocarInstanciaConfiguracao<T>(object[] args, T modulo) where T : IModule
        {
            MethodInfo mthInstancia = modulo.GetType().GetMethod(Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            _ = args.GetType() == typeof(string[]) ? (mthInstancia?.Invoke(modulo, new object[] { args })) : (mthInstancia?.Invoke(modulo, args));

            MethodInfo mthConfiguracao = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_CONFIGURACAO);
            _ = (mthConfiguracao?.Invoke(modulo, Array.Empty<object>()));
        }

    }
}
