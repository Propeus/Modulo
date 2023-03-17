using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Thread;


namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public sealed class Gerenciador : ModeloBase, IGerenciador, IGerenciadorRegistro, IGerenciadorInformacao, IGerenciadorDiagnostico
    {
        /// <summary>
        /// Inicializa o gerenciador de modulos
        /// </summary>
        private Gerenciador()
        {
            DataInicio = DateTime.Now;

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;


        }

        private readonly CancellationTokenSource _cancellationToken = new();
        private static Gerenciador _atual;


        ///<inheritdoc/>
        public static IGerenciador Atual
        {
            get
            {
                if (_atual is null || _atual.Estado == Estado.Desligado)
                {
                    _atual = new Gerenciador();
                }

                return _atual;
            }
        }

        ///<inheritdoc/>
        public DateTime DataInicio { get; }
        ///<inheritdoc/>
        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        ///<inheritdoc/>
        public int ModulosInicializados => InstanciaProvider.Get().Count();

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
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
        ///Exemplo para criar um modulo com contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo =  (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato");
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Criar(string nomeModulo)
        {
            Type result = TypeProvider.Get(nomeModulo);
            return result == null
                ? throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : Criar(result);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
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
        ///Exemplo para criar um modulo com contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
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
        ///Exemplo para criar um modulo com contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
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


            modulo = ResolverContrato(modulo);


            if (InstanciaProvider.HasCache(modulo.Name))
            {
                throw new ModuloInstanciaUnicaException(Constantes.ERRO_MODULO_INSTANCIA_UNICA);
            }

            ConstructorInfo ctor = modulo.GetConstructors().MaxBy(x => x.GetParameters().Length);
            if (ctor is null)
            {
                throw new ModuloConstrutorAusenteException(Constantes.ERRO_CONSTRUTOR_NAO_ENCONTRADO);
            }

            ParameterInfo[] paramCtor = ctor.GetParameters();
            object[] arr = new object[paramCtor.Length];

            for (int i = 0; i < paramCtor.Length; i++)
            {
                if (!paramCtor[i].ParameterType.Is<IGerenciador>() && (paramCtor[i].ParameterType.Is<IModulo>() || paramCtor[i].ParameterType.PossuiAtributo<ModuloContratoAttribute>()))
                {
                    if (!Existe(paramCtor[i].ParameterType))
                    {
                        if (paramCtor[i].IsOptional)
                        {
                            try
                            {
                                arr[i] = Criar(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                            }
                            catch (TipoModuloNaoEncontradoException)
                            {
                                arr[i] = paramCtor[i].ParameterType.Default();
                            }
                        }
                        else
                        {
                            arr[i] = Criar(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                        }

                    }
                    else
                    {
                        arr[i] = Obter(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                }
                else if (paramCtor[i].ParameterType.Is<IGerenciador>())
                {
                    IModuloTipo gen = InstanciaProvider.Get().FirstOrDefault(x => x.Modulo is IGerenciador);
                    arr[i] = (gen?.Modulo as IGerenciador) ?? this;
                }
                else
                {
                    if (paramCtor[i].IsOptional)
                    {
                        continue;
                    }

                    throw new TipoModuloInvalidoException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Modulo, Contrato ou Gerenciador");
                }
            }

            return InstanciaProvider.Create(modulo, arr);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<example>
        ///Exemplo para verificar um modulo
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(gerenciador.Existe(typeof(ICalculadoraModuloContrato)));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            try
            {
                modulo = ResolverContrato(modulo);
                return InstanciaProvider.HasByName(modulo.Name);
            }
            catch (Exception)
            {
                return false;
            }


        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///Exemplo para criar um modulo com contrato
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(gerenciador.Existe(modulo));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(IModulo modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : InstanciaProvider.HasById(modulo.Id);
        }
        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(gerenciador.Existe(modulo.Id));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public bool Existe(string id)
        {
            return string.IsNullOrEmpty(id) ? throw new ArgumentNullException(nameof(id)) : InstanciaProvider.HasById(id);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi encontrado</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo(typeof(ICalculadoraModuloContrato)));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModuloTipo ObterInfo(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            if (!modulo.IsInterface && !modulo.IsClass)
            {
                throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_INVALIDO);
            }

            modulo = ResolverContrato(modulo);

            IModuloTipo info = InstanciaProvider.GetByName(modulo.Name);

            return info is null
                ? throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, modulo.Name))
                : info;
        }

        ///<inheritdoc/>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo&lt;ICalculadoraModuloContrato&gt;());
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModuloTipo ObterInfo<T>() where T : IModulo
        {
            return ObterInfo(typeof(T));
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo(modulo.Id));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModuloTipo ObterInfo(string id)
        {
            if (InstanciaProvider.HasById(id))
                return InstanciaProvider.GetById(id);
            else
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, id));
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo <see cref="GC"/> ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(gerenciador.Obter(typeof(ICalculadoraModuloContrato)));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Obter(Type modulo)
        {
            modulo = ResolverContrato(modulo);

            return InstanciaProvider.GetByName(modulo.Name).Modulo;
        }

        ///<inheritdoc/>
        ///<exception cref="TipoModuloInvalidoException">Tipo do modulo invalido</exception>
        ///<exception cref="ModuloContratoNaoEncontratoException">Tipo da interface de contrato nao possui o atributo <see cref="ModuloContratoAttribute"/></exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.Obter&lt;ICalculadoraModuloContrato&gt;());
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
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo G.C ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
        ///                Console.WriteLine(gerenciador.Obter(modulo.Id));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public IModulo Obter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), string.Format(Constantes.ERRO_ARGUMENTO_NULO_OU_VAZIO, nameof(id)));
            }

            if (!Existe(id))
            {
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, id));
            }

            IModuloTipo info = InstanciaProvider.GetById(id);


            return info.Elimindado ? throw new ModuloDescartadoException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, id)) : info.Modulo;
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.Remover(modulo));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void Remover<T>(T modulo) where T : IModulo
        {
            InstanciaProvider.Remove(modulo);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo G.C ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.Remover(modulo.Id));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void Remover(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"'{nameof(id)}' não pode ser nulo nem vazio.", nameof(id));
            }

            if (!InstanciaProvider.RemoveById(id))
            {
                throw new ModuloNaoEncontradoException("Modulo nao encontrdo");
            }
        }

        ///<inheritdoc/>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.RemoverTodos());
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public void RemoverTodos()
        {
            InstanciaProvider.Flush();
        }


        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoRegistradoException">Modulos criados fora do gerenciador</exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.Reciclar&lt;ICalculadoraModuloContrato&gt;());
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public T Reciclar<T>(T modulo) where T : IModulo
        {
            return (T)Reciclar(modulo.Id);
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloNaoEncontradoException">Instancia do modulo nao foi inicializado</exception>
        ///<exception cref="ModuloDescartadoException">Instancia do modulo foi coletado pelo G.C ou acionou o <see cref="IDisposable.Dispose"/></exception>
        ///<example>
        ///<code>
        ///using System;
        ///using Propeus.Modulo.Abstrato.Atributos;
        ///using Propeus.Modulo.Core.Gerenciador;
        ///
        ///namespace Propeus.Modulo.Exemplo
        ///{
        ///    [Modulo]
        ///    public class CalculadoraModulo : ICalculadoraModuloContrato
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
        ///
        ///    [ModuloContrato(typeof(CalculadoraModulo))]
        ///    public interface ICalculadoraModuloContrato : IModulo
        ///    {
        ///        public int Calcular(int a, int b);
        ///    }
        ///    
        ///    internal class Program
        ///    {
        ///        private static void Main(string[] args)
        ///        {
        ///            using(Gerenciador gerenciador = Gereciador.Atual)
        ///            {
        ///                ICalculadoraModuloContrato modulo = gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                Console.WriteLine(gerenciador.Reciclar(modulo.Id));
        ///            }
        ///        }
        ///    }
        ///}
        /// </code>
        ///</example>
        public IModulo Reciclar(string id)
        {
            if (InstanciaProvider.HasById(id))
            {
                var moduloTipo = InstanciaProvider.GetById(id).TipoModulo;
                InstanciaProvider.RemoveById(id);
                return Criar(moduloTipo);
            }
            else
            {
                throw new ModuloNaoEncontradoException(Constantes.ERRO_MODULO_NEW_REINICIAR);
            }
        }

        ///<inheritdoc/>
        ///<exception cref="ArgumentNullException">Parametro nulo</exception>
        ///<exception cref="ModuloInstanciaUnicaException">Criacao de mais de uma instancia de modulo definido como instancia unica</exception>
        ///<exception cref="ModuloRegistradoException">O modulo de mesmo Id ja foi registrado</exception>
        public void Registrar(IModulo modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            InstanciaProvider.Register(modulo);
        }

        ///<inheritdoc/>
        ///<example>
        /// 
        /// Exemplo de como manter o gerenciador em modo interativo
        /// <code>
        /// using System;
        /// using Propeus.Modulo.Core.Gerenciador
        /// 
        /// namespace Propeus.Modulo.Exemplo
        /// {
        ///     internal class Program
        ///     {
        ///         private static void Main(string[] args)
        ///         {
        ///             using(Gerenciador gerenciador = Gereciador.Atual)
        ///             {
        ///                 gerenciador.ManterVivoAsync().Wait();
        ///             }
        ///         }
        ///     }
        ///}
        /// </code>
        ///</example>
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
        ///<example>
        ///Exemplo para criar um modulo com contrato
        ///
        ///Exemplo de codigo do contrato
        ///<code>
        ///using Propeus.Modulo.Abstrato.Atributos;
        /// 
        /// namespace Propeus.Modulo.Exemplo
        /// {
        ///     [ModuloContrato(typeof(CalculadoraModulo))]
        ///     public interface ICalculadoraModuloContrato : IModulo
        ///     {
        ///         public int Calcular(int a, int b);
        ///     }
        /// }
        /// </code>
        /// 
        /// Exemplo de codigo do modulo
        /// <code>
        /// using Propeus.Modulo.Abstrato.Atributos;
        ///
        /// namespace Propeus.Modulo.Exemplo
        /// {
        ///     [Modulo]
        ///     public class CalculadoraModulo : ICalculadoraModuloContrato
        ///     {
        ///     
        ///         public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        ///         {
        ///         
        ///         }
        ///         
        ///         public int Calcular(int a, int b)
        ///         {
        ///             return a+b;
        ///         }
        ///     }
        /// }
        /// </code>
        /// 
        /// Exemplo de como reiniciar uma instancia do modulo pelo ID
        /// <code>
        /// using System;
        /// using Propeus.Modulo.Core.Gerenciador
        /// 
        /// namespace Propeus.Modulo.Exemplo
        /// {
        ///     internal class Program
        ///     {
        ///         private static void Main(string[] args)
        ///         {
        ///             using(Gerenciador gerenciador = Gereciador.Atual)
        ///             {
        ///                 ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar&lt;ICalculadoraModuloContrato&gt;();
        ///                 IEnumerable&lt;IModulos&gt; modulos = gerenciador.Listar();
        ///                 foreach(IModulo fModulo in modulos)
        ///                 {
        ///                     Console.WriteLine(fModulo);
        ///                 }
        ///             }
        ///         }
        ///     }
        ///}
        /// </code>
        ///</example>
        public IEnumerable<IModulo> Listar()
        {
            return InstanciaProvider.Get().Select(x => x.Modulo);
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


                modulo = attr.Tipo ?? TypeProvider.Get(attr.Nome);
                if (modulo is null)
                {
                    throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, attr.Nome));
                }

            }

            if (modulo.IsClass)
            {
                if (!modulo.Is<IModulo>())
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
                RemoverTodos();
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
#pragma warning disable S2696
                _atual = null;
#pragma warning restore S2696

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
