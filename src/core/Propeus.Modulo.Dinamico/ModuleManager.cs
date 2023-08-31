using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Propeus.Module.Watcher.Contracts;
using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Exceptions;
using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Modulo.IL.Core.Geradores;
using Propeus.Modulo.IL.Core.Helpers;
using Propeus.Modulo.Util.Atributos;

namespace Propeus.Modulo.Dinamico
{



    /// <summary>
    /// ModuleProxy responsável por administrar modulos em tempo de execução
    /// </summary>
    [Module]
    public class ModuleManager : BaseModule, IModuleManagerArguments
    {

        /// <summary>
        /// Inicializa o gerenciador
        /// </summary>
        /// <param name="moduleManager">Gerenciador que irá controlar o modulo</param>
        ///<example>
        ///Criar uma instancia do gerenciador dinamico
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
        ///{
        ///  internal class Program
        ///  {
        ///      private static void Main()
        ///      {
        ///         using (gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///         {
        ///            //Seu codigo aqui...
        ///         }
        ///      }
        ///  }
        ///}
        ///</code>
        ///</example>
        public ModuleManager(IModuleManager moduleManager) : base()
        {
            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            StartDate = DateTime.Now;

            _gerenciador = moduleManager;
        }



        private readonly IModuleManager _gerenciador;

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

        private IModule CreateModuleNonConfigurated(Type moduleType)
        {
            if (moduleType is null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            if (moduleType.IsInterface)
            {
                moduleType = ResolveContract(moduleType);
            }


            ConstructorInfo? ctor = moduleType.GetConstructors()
                .MaxBy(cto => cto.GetParameters().Length) ?? throw new ModuleBuilderAbsentException(moduleType);

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
                        catch (ModuleContractInvalidException)
                        {
                            //Ignora erro neste caso
                        }
                    }
                    else
                    {
                        if (!ExistsModule(param.ParameterType))
                            _ = CreateModule(param.ParameterType);
                    }
                }

            }

            var module = _gerenciador.CreateModule(moduleType);
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule&gt;ICalculadoraModuloContrato&lt;();
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
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///<note type="tip">
        ///Um module não precisa obrigatoriamente possuir uma interface de contrato, porém é recomendável.
        ///</note>
        ///<note type="important">
        /// O retorno deste método sempre será <see cref="IModule"/>, tome cuidado ao realizar o cast para um tipo não compatível.
        /// </note>
        ///</example>
        public IModule CreateModule(Type moduleType)
        {
            var module = CreateModuleNonConfigurated(moduleType);
            InvocarInstanciaConfiguracao(Array.Empty<object>(), module);
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato");
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
        public IModule CreateModule(string moduleName)
        {
            var module = CreateModuleNonConfigurated(GetModule<IModuleWatcherContract>()[moduleName]);
            InvocarInstanciaConfiguracao(Array.Empty<object>(), module);
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
        ///<exception cref="ModuleBuilderAbsentException">Construtor ausente no modulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManagerArguments gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        public T CreateModule<T>(object[] args) where T : IModule
        {
            T modulo = (T)CreateModuleNonConfigurated(typeof(T));

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
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManagerArguments gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        public IModule CreateModule(Type moduleType, object[] args)
        {
            IModule iModulo = CreateModuleNonConfigurated(moduleType);
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
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManagerArguments gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        public IModule CreateModule(string moduleName, object[] args)
        {
            var moduleWatcher = GetModule<IModuleWatcherContract>();
            var module = CreateModuleNonConfigurated(moduleWatcher[moduleName]);
            InvocarInstanciaConfiguracao(args, module);
            return module;
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///           using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.CreateModule("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.RemoveAllModules();
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
        ///using Propeus.Module.Abstract;
        ///using Propeus.Module.Abstract.Attributes;
        ///using Propeus.Module.Abstract.Interfaces;
        ///using Propeus.Module.Manager;
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///using Propeus.Modulo.Dinamico;
        ///
        ///namespace Propeus.Modulo.Example
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
        ///            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManagerDefault(Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager()))
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
                RemoveAllModules();

                foreach (KeyValuePair<string, ILClasseProvider> item in ModuloProvider)
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
                return _gerenciador.GetModule<IModuleWatcherContract>().GetModuleFromContract(contractType);
            }

        }
        private static void InvocarInstanciaConfiguracao<T>(object[] args, T modulo) where T : IModule
        {


            MethodInfo? mthInstancia = modulo.GetType().GetMethod(Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            _ = args.GetType() == typeof(string[]) ? (mthInstancia?.Invoke(modulo, new object[] { args })) : (mthInstancia?.Invoke(modulo, args));

            MethodInfo? mthConfiguracao = modulo.GetType().GetMethod(Constantes.METODO_CONFIGURACAO);
            _ = (mthConfiguracao?.Invoke(modulo, Array.Empty<object>()));
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
    }
}
