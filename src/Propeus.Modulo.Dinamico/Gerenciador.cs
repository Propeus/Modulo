﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;
using Propeus.Modulo.Util.Thread;
using Propeus.Modulo.Abstrato.Exceptions;
using System.Reflection;
using Propeus.Modulo.Abstrato.Proveders;

namespace Propeus.Modulo.Dinamico
{
    /// <summary>
    /// Modulo responsável por administrar modulos em tempo de execução
    /// </summary>
    [Modulo]
    public class Gerenciador : ModuloBase, IGerenciador, IGerenciadorArgumentos, IGerenciadorDiagnostico, IGerenciadorRegistro, IGerenciadorInformacao
    {
        /// <summary>
        /// Inicializa o gerenciador
        /// </summary>
        /// <param name="gerenciador">Gerenciador que irá controlar o modulo</param>
        /// <param name="configuracao">Configuracao do gerenciador atual</param>
        public Gerenciador(IGerenciador gerenciador, GerenciadorConfiguracao configuracao = null) : base(gerenciador, true)
        {

            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            DataInicio = DateTime.Now;
            _scheduler = new TaskJob(3);
            Configuracao = configuracao ?? new GerenciadorConfiguracao();


            CARREGAR_MODULO_JOB(null);

            _scheduler.AddTask(CARREGAR_MODULO_JOB, TimeSpan.FromSeconds(1), "CARREGAR_MODULO_JOB");
            _scheduler.AddTask(AUTO_INICIALIZAR_MODULO_JOB, TimeSpan.FromSeconds(1), "AUTO_INICIALIZAR_MODULO_JOB");

        }

        private TaskJob _scheduler;

        /// <summary>
        /// Configuracoes do gerenciador
        /// </summary>
        public GerenciadorConfiguracao Configuracao { get; }
        /// <summary>
        /// Diretório atual do modulo
        /// </summary>
        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();
        ///<inheritdoc/>
        public DateTime DataInicio { get; }
        ///<inheritdoc/>
        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int ModulosInicializados => ((Gerenciador is IGerenciadorDiagnostico) ? (Gerenciador as IGerenciadorDiagnostico).ModulosInicializados : -1);
        private Dictionary<string, ILClasseProvider> ModuloProvider { get; }



        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar&gt;ICalculadoraModuloContrato&lt;();
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(modulo.Calcular(1,1));
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

            //TODO: Obter construtor de maior quantidade de parametros
            var ctors = modulo.GetConstructors();
            foreach (var ctor in ctors)
            {
                var @params = ctor.GetParameters();
                foreach (var @param in @params)
                {
                    if (param.ParameterType.IsInterface && param.ParameterType.PossuiAtributo<ModuloContratoAttribute>())
                    {
                        if (param.IsOptional)
                        {
                            try
                            {
                                this.ResoverContratos(param.ParameterType);
                            }
                            catch (ModuloNaoEncontradoException)
                            {
                                //Ignora erro neste caso
                            }
                        }
                        else
                        {
                            this.ResoverContratos(param.ParameterType);
                        }
                    }
                }
            }

            return Gerenciador.Criar(modulo);


        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato");
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar&gt;ICalculadoraModuloContrato&lt;(new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Criar<T>(object[] args) where T : IModulo
        {


            T modulo = (T)Criar(typeof(T));

            var mthInstancia = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());

            if (args.GetType() == typeof(string[]))
            {
                mthInstancia?.Invoke(modulo, new object[] { args });
            }
            else
            {
                mthInstancia?.Invoke(modulo, args);
            }

            var mthConfiguracao = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_CONFIGURACAO);
            mthConfiguracao?.Invoke(modulo, Array.Empty<object>());

            return modulo;

        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato),new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(Type modulo, object[] args)
        {
            IModulo iModulo = Criar(modulo);

            var mthInstancia = iModulo.GetType().GetMethod(Abstrato.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            if (args.GetType() == typeof(string[]))
            {
                mthInstancia?.Invoke(iModulo, new object[] { args });
            }
            else
            {
                mthInstancia?.Invoke(iModulo, args);
            }

            var mthConfiguracao = modulo.GetType().GetMethod(Abstrato.Constantes.METODO_CONFIGURACAO);
            mthConfiguracao?.Invoke(modulo, Array.Empty<object>());

            return iModulo;


        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ArgumentException">O tipo informado não é uma interface</exception>
        ///<exception cref="InvalidCastException">O tipo nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao herdado de <see cref="IModulo"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo nao possui o atributo <see cref="ModuloAttribute"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Parametro do construtor nao e um modulo valido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo nao encontrado pelo nome no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="TipoModuloNaoEncontradoException">Tipo ausente no atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloConstrutorAusenteException">Construtor ausente no modulo</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(string nomeModulo, object[] args)
        {
            IModulo iModulo = Criar(Nome);

            var mthInstancia = iModulo.GetType().GetMethod(Abstrato.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            if (args.GetType() == typeof(string[]))
            {
                mthInstancia?.Invoke(iModulo, new object[] { args });
            }
            else
            {
                mthInstancia?.Invoke(iModulo, args);
            }

            var mthConfiguracao = iModulo.GetType().GetMethod(Abstrato.Constantes.METODO_CONFIGURACAO);
            mthConfiguracao?.Invoke(iModulo, Array.Empty<object>());

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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.Remover(modulo);
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
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.Remover(modulo.Id);
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.Recilcar(modulo);
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
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Recilcar(modulo.Id);
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
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Obter&gt;ICalculadoraModuloContrato&lt;();
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Obter(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Obter(Type modulo)
        {
            return Gerenciador.Obter(modulo);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Obter(modulo.Id);
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
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Existe(typeof(ICalculadoraModuloContrato));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(Type modulo)
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Existe(modulo);
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                modulo =  gerenciador.Existe(modulo.Id);
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
            this._scheduler.Dispose();
            base.Dispose(disposing);
            Gerenciador.Dispose();
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
            //_ = stringBuilder.Append("Tempo de atualização dos caminho_modulos: ").Append(_tempoAtualziacaoModulo).Append(" segundos").AppendLine();

            return stringBuilder.ToString();
        }

        private void AUTO_INICIALIZAR_MODULO_JOB(object cancelationToken)
        {
            //Como autoinicialziar agora?
            foreach (var modulo in TypeProvider.ObterAutoInicializavel())
            {
                if (!Existe(modulo))
                {
                    Criar(modulo);
                }
            }

        }
        private void CARREGAR_MODULO_JOB(object cancelationToken)
        {

            ModuleProvider.RecarregmentoCompleto();

        }

        ///<inheritdoc/>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do modulo
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi encontrado</exception>
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do modulo
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
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
        ///          //Voce pode definir quantos parametros e tipos, portanto que seja compativel com a quantidade de argumentos informados ao criar o modulo.
        ///          //Caso contrario, este metodo nao será invocado
        ///        }
        ///        
        ///    }
        ///}
        ///</code>
        ///No projeto principal, adicione uma interface de contrato e depois obtenha as informacoes basicas do modulo
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
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato",new object[]{1,"Um valor qualquer para chamar a funcao CriarInstancia"});
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///                gerenciador.ObterInfo(modulo.Id);
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
                throw new ArgumentException("O tipo nao e uma interface");

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
                    ModuloProvider[tipoImplementacao.Name].NovaVersao(interfaces: TypeProvider.ObterContratos(tipoImplementacao).ToArray()).CriarProxyClasse(tipoImplementacao,TypeProvider.ObterContratos(tipoImplementacao).ToArray());
                }

                ModuloProvider[tipoImplementacao.Name].Executar();
                contrato = ModuloProvider[tipoImplementacao.Name].ObterTipoGerado();

                TypeProvider.AddOrUpdate(contrato);

                return contrato;


            }
            else
            {
                //TODO: Interface nao mapeada
                throw new InvalidCastException();
            }
        }
    }
}
