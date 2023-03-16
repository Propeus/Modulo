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
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Dinamico
{
    /// <summary>
    /// Modulo responsável por administrar modulos em tempo de execução
    /// </summary>
    [Modulo]
    public class Gerenciador : ModuloBase, IGerenciadorArgumentos, IGerenciadorDiagnostico, IGerenciadorRegistro, IGerenciadorInformacao
    {
        /// <summary>
        /// Inicializa o gerenciador
        /// </summary>
        /// <param name="gerenciador">Gerenciador que irá controlar o type</param>
        /// <param name="configuracao">Configuracao do gerenciador atual</param>
        public Gerenciador(IGerenciador gerenciador, GerenciadorConfiguracao configuracao = null) : base(gerenciador, true)
        {

            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            DataInicio = DateTime.Now;
            _scheduler = new TaskJob(3);
            Configuracao = configuracao ?? new GerenciadorConfiguracao();


            CARREGAR_MODULO_JOB(null);

            _ = _scheduler.AddTask(CARREGAR_MODULO_JOB, TimeSpan.FromSeconds(1), "CARREGAR_MODULO_JOB");
            _ = _scheduler.AddTask(AUTO_INICIALIZAR_MODULO_JOB, TimeSpan.FromSeconds(1), "AUTO_INICIALIZAR_MODULO_JOB");

        }

        private readonly TaskJob _scheduler;

        /// <summary>
        /// Configuracoes do gerenciador
        /// </summary>
        public GerenciadorConfiguracao Configuracao { get; }
        /// <summary>
        /// Diretório atual do type
        /// </summary>
        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();
        ///<inheritdoc/>
        public DateTime DataInicio { get; }
        ///<inheritdoc/>
        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int ModulosInicializados => (Gerenciador is IGerenciadorDiagnostico) ? (Gerenciador as IGerenciadorDiagnostico).ModulosInicializados : -1;
        private Dictionary<string, ILClasseProvider> ModuloProvider { get; }



        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar&gt;ICalculadoraModuloContrato&lt;();
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Criar<T>() where T : IModulo
        {
            return (T)Criar(typeof(T));
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            if (modulo.IsInterface)
            {
                modulo = ResoverContratos(modulo);
            }

            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            ConstructorInfo ctor = modulo.GetConstructors().MaxBy(cto => cto.GetParameters().Length);

            ParameterInfo[] @params = ctor.GetParameters();
            foreach (ParameterInfo @param in @params)
            {
                if (param.ParameterType.IsInterface && param.ParameterType.PossuiAtributo<ModuloContratoAttribute>())
                {
                    if (param.IsOptional)
                    {
                        try
                        {
                            _ = ResoverContratos(param.ParameterType);
                        }
                        catch (ModuloNaoEncontradoException)
                        {
                            //Ignora erro neste caso
                        }
                    }
                    else
                    {
                        _ = ResoverContratos(param.ParameterType);
                    }
                }

            }

            return Gerenciador.Criar(modulo);


        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato");
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(string nomeModulo)
        {
            return Criar(ModuleProvider.ObterTipoModuloAtual(nomeModulo));
        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar&gt;ICalculadoraModuloContrato&lt;(new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Criar<T>(object[] args) where T : IModulo
        {


            T modulo = (T)Criar(typeof(T));

            InvocarInstanciaConfiguracao(args, modulo);

            return modulo;

        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato),new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(Type modulo, object[] args)
        {
            IModulo iModulo = Criar(modulo);

            InvocarInstanciaConfiguracao(args, iModulo);

            return iModulo;


        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um type valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de type definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no type</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(string nomeModulo, object[] args)
        {
            IModulo iModulo = Criar(Nome);

            InvocarInstanciaConfiguracao(args, iModulo);

            return iModulo;
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois remova
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.Remover(type);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void Remover<T>(T modulo) where T : IModulo
        {
            Gerenciador.Remover(modulo);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do type foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois remova pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.Remover(type.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void Remover(string id)
        {
            Gerenciador.Remover(id);
        }
        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois remova pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.RemoverTodos();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void RemoverTodos()
        {
            Gerenciador.RemoverTodos();
        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois recicle
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.Recilcar(type);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Reciclar<T>(T modulo) where T : IModulo
        {
            T nModulo = Gerenciador.Reciclar(modulo);
            return nModulo;
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do type foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois recicle pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Recilcar(type.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Reciclar(string id)
        {
            return Gerenciador.Reciclar(id);
        }


        ///<inheritdoc/>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi inicializado</exception>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Obter&gt;ICalculadoraModuloContrato&lt;();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Obter<T>() where T : IModulo
        {
            return (T)Obter(typeof(T));
        }
        ///<inheritdoc/>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Obter(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Obter(Type type)
        {
            return Gerenciador.Obter(type);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi inicializado</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtem pelo tipo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Obter(type.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Obter(string id)
        {
            return Gerenciador.Obter(id);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Existe(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(Type type)
        {
            return Gerenciador.Existe(type);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Existe(type);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(IModulo modulo)
        {
            return Gerenciador.Existe(modulo);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia pelo ID
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                type =  gerenciador.Existe(type.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(string id)
        {
            return Gerenciador.Existe(id);
        }

        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois verifica se existe a instancia
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                IEnumerable&gt;IModulo&lt; =  gerenciador.Listar();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IEnumerable<IModulo> Listar()
        {
            return Gerenciador.Listar();
        }

        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois mantenha vivo a instancia do gerenciador
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.ManterVivoAsync().Wait();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public async Task ManterVivoAsync()
        {
            await Gerenciador.ManterVivoAsync().ConfigureAwait(true);
        }

        ///<inheritdoc/>
        public void Registrar(IModulo modulo)
        {
            (Gerenciador as IGerenciadorRegistro).Registrar(modulo);
        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            RemoverTodos();
            _scheduler.Dispose();
            base.Dispose(disposing);
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());



            _ = stringBuilder.Append("Data de inicializacao: ").Append(DataInicio).AppendLine();
            _ = stringBuilder.Append("Tempo em execução: ").Append(DateTime.Now - DataInicio).AppendLine();
            _ = stringBuilder.Append("Jobs em execucao: ").Append(_scheduler.EmExecucao).AppendLine();

            _ = stringBuilder.Append("Caminho do diretório: ").Append(DiretorioModulo).AppendLine();
            _ = stringBuilder.Append("Quantidade de DLLs no diretório: ").Append(ModuleProvider.ModulosDllCarregados).AppendLine();
            _ = stringBuilder.Append("Quantidade de caminho_modulos inicializados: ").Append(ModulosInicializados).AppendLine();

            return stringBuilder.ToString();
        }

        private void AUTO_INICIALIZAR_MODULO_JOB(object cancelationToken)
        {
            //Como autoinicialziar agora?
            foreach (Type modulo in TypeProvider.ObterAutoInicializavel())
            {
                if (!Existe(modulo))
                {
                    _ = Criar(modulo);
                }
            }

        }
        private void CARREGAR_MODULO_JOB(object cancelationToken)
        {

            ModuleProvider.RecarregmentoCompleto();

        }

        ///<inheritdoc/>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi inicializado</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do type
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.ObterInfo&gt;ICalculadoraModuloContrato&lt;();
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        IModuloTipo IGerenciadorInformacao.ObterInfo<T>()
        {
            return (Gerenciador as IGerenciadorInformacao).ObterInfo<T>();
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do type invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do type nao foi encontrado</exception>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do type
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.ObterInfo(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        IModuloTipo IGerenciadorInformacao.ObterInfo(Type type)
        {
            return (Gerenciador as IGerenciadorInformacao).ObterInfo(type);
        }

        ///<inheritdoc/>
        ///<example>
        ///Crie uma classe em um projeto separado
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ModuloBase
        ///    {
        ///        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o type.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do type
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///using Propeus.Modulo.Dinamico.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///
        ///    [ModuloContrato("CalculadoraModulo")]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = new Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual))
        ///            {
        ///                ICalculadoraModuloContrato type = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(type.Calcular(1,1));
        ///                gerenciador.ObterInfo(type.Id);
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        IModuloTipo IGerenciadorInformacao.ObterInfo(string id)
        {
            return (Gerenciador as IGerenciadorInformacao).ObterInfo(id);
        }

        /// <summary>
        /// Obtem o tipo implementado com base na interface informada
        /// </summary>
        /// <param name="contrato"></param>
        /// <returns>Tipo implementado</returns>
        /// <exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        /// <exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        private Type ResoverContratos(Type contrato)
        {
            if (!contrato.IsInterface)
            {
                throw new ArgumentException("O tipo nao e uma interface");
            }

            if (contrato.PossuiAtributo<ModuloContratoAttribute>())
            {
                Type tipoImplementacao;
                ILClasseProvider provider;

                ModuloContratoAttribute attr = contrato.ObterAtributo<ModuloContratoAttribute>();

                tipoImplementacao = attr.Tipo ?? ModuleProvider.ObterTipoModuloAtual(attr.Nome);
                if (tipoImplementacao == null)
                {
                    throw new ModuloNaoEncontradoException("Modulo nao encontrado");
                }

                TypeProvider.AdicionarContrato(tipoImplementacao, contrato);

                if (!ModuloProvider.ContainsKey(tipoImplementacao.Name))
                {
                    provider = GeradorHelper.ObterModuloGenerico().CriarProxyClasse(tipoImplementacao, TypeProvider.ObterContratos(tipoImplementacao).ToArray());
                    ModuloProvider.Add(tipoImplementacao.Name, provider);
                }
                else
                {
                    _ = ModuloProvider[tipoImplementacao.Name].NovaVersao(interfaces: TypeProvider.ObterContratos(tipoImplementacao).ToArray()).CriarProxyClasse(tipoImplementacao, TypeProvider.ObterContratos(tipoImplementacao).ToArray());
                }

                ModuloProvider[tipoImplementacao.Name].Executar();
                contrato = ModuloProvider[tipoImplementacao.Name].ObterTipoGerado();

                TypeProvider.AddOrUpdate(contrato);

                return contrato;


            }
            else
            {
                throw new ModuloContratoNaoEncontratoException("Atributo nao encontrado no tipo informado");
            }
        }
        private static void InvocarInstanciaConfiguracao<T>(object[] args, T modulo) where T : IModulo
        {
            MethodInfo mthInstancia = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            _ = args.GetType() == typeof(string[]) ? (mthInstancia?.Invoke(modulo, new object[] { args })) : (mthInstancia?.Invoke(modulo, args));

            MethodInfo mthConfiguracao = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_CONFIGURACAO);
            _ = (mthConfiguracao?.Invoke(modulo, Array.Empty<object>()));
        }

    }
}
