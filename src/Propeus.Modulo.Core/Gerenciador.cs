﻿using System;
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

            Workers = new TaskJob();

            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Workers.AddTask((cts) =>
            {

                CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)cts;

                foreach (var key in Modulos.Keys)
                {
                    if (Modulos[key].Elimindado)
                    {
                        Cache.TryRemove(key, out string _);
                        Modulos.TryRemove(key, out IModuloTipo _);
                    }
                }
            }, TimeSpan.FromSeconds(1), "LIMPEZA_AUTOMATICA_WORKER");
        }

        private readonly CancellationTokenSource _cancellationToken = new();
        private static Gerenciador _atual;
        private TaskJob Workers { get; set; }
        /// <summary>
        /// Dicionario composto por ID do modulo e instancia do tipo do modulo
        /// </summary>
        private ConcurrentDictionary<string, IModuloTipo> Modulos { get; } = new ConcurrentDictionary<string, IModuloTipo>();
        ///<inheritdoc/>
        private ConcurrentDictionary<string, string> Cache { get; } = new ConcurrentDictionary<string, string>();

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
        public int ModulosInicializados => Modulos.Count;

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


            if (Cache.ContainsKey(modulo.FullName))
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
                        arr[i] = Criar(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                    else
                    {
                        arr[i] = Obter(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                }
                else if (paramCtor[i].ParameterType.Is<IGerenciador>())
                {
                    var gen = Modulos.FirstOrDefault(x => x.Value.Modulo is IGerenciador).Value;
                    arr[i] = (gen?.Modulo as IGerenciador) ?? this;
                }
                else
                {
                    if (paramCtor[i].IsOptional)
                        continue;

                    throw new TipoModuloInvalidoException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Modulo, Contrato ou Gerenciador");
                }
            }

            return (IModulo)Activator.CreateInstance(modulo, arr);

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
        public bool Existe(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            bool result = false;

            if (type.IsInterface)
            {

                ModuloContratoAttribute contrato = type.ObterModuloContratoAtributo();
                if (contrato is null)
                {
                    throw new ModuloContratoNaoEncontratoException(Constantes.ERRO_MODULO_CONTRATO_NAO_ENCONTRADO);
                }

                if (Cache.TryGetValue(contrato.Nome, out string idOuter))
                {
                    result = Modulos.ContainsKey(idOuter);
                }
            }
            else if (type.IsClass)
            {
                if (Cache.TryGetValue(type.FullName, out string idOuter))
                {
                    result = Modulos.ContainsKey(idOuter);
                }

                result = this.Modulos.Values.Any(t => t.TipoModulo == type);
            }
            else
            {
                throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_INVALIDO);
            }

            return result;
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
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : Existe(modulo.Id);
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
            return string.IsNullOrEmpty(id) ? throw new ArgumentNullException(nameof(id)) : Modulos.ContainsKey(id);
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
                throw new ArgumentNullException(nameof(modulo), message: string.Format(Constantes.ARGUMENTO_NULO, nameof(modulo)));
            }

            if (!modulo.IsInterface && !modulo.IsClass)
            {
                throw new TipoModuloInvalidoException(Constantes.ERRO_TIPO_INVALIDO);
            }

            modulo = ResolverContrato(modulo);

            IModuloTipo info = null;

            if (modulo.IsClass)
            {
                if (Cache.TryGetValue(modulo.FullName, out string idOuter))
                {
                    if (Modulos.TryGetValue(idOuter, out IModuloTipo infoOuter))
                    {
                        info = infoOuter;
                    }
                }

                info ??= Modulos.Values.FirstOrDefault(x => x.TipoModulo == modulo);
            }


            if (info is null)
            {
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_NAO_ENCONTRADO, modulo.Name));
            }

            return info;
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
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!Existe(id))
            {
                throw new ModuloNaoEncontradoException(string.Format(Constantes.ERRO_MODULO_ID_NAO_ENCONTRADO, id));
            }

            return Modulos[id];
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
            var result = ObterInfo(modulo);
            return result.Elimindado ? throw new ModuloDescartadoException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, result.Nome)) : result.Modulo;
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

            IModuloTipo info = Modulos[id];


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
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo), message: string.Format(Constantes.ARGUMENTO_NULO, nameof(modulo)));
            }

            Type t = modulo.GetType();
            _ = Cache.TryRemove(t.FullName, out _);
            _ = Cache.TryRemove(t.Name, out _);
            if (Modulos.TryRemove(modulo.Id, out IModuloTipo outer))
            {
                outer.Dispose();
            }

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
            Remover(Obter(id));
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

            foreach (KeyValuePair<string, IModuloTipo> modulo in Modulos)
            {

                if (!modulo.Value.Elimindado)
                {
                    Remover(modulo.Value.Modulo);
                }
                else
                {
                    _ = Cache.TryRemove(modulo.Value.Nome, out _);
                    _ = Modulos.TryRemove(modulo.Value.IdModulo, out _);
                }

            }
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
            if (this.Modulos.ContainsKey(modulo.Id))
            {
                Remover(modulo);
                return (T)Criar(modulo.GetType());
            }
            else
            {
                throw new ModuloNaoRegistradoException(Constantes.ERRO_MODULO_NEW_REINICIAR);
            }

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
            IModulo modulo = Obter(id);
            Remover(id);
            return Criar(modulo.GetType());
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

            Type t = modulo.GetType();
            if (modulo.InstanciaUnica)
            {
                if (Cache.ContainsKey(t.FullName))
                {
                    throw new ModuloInstanciaUnicaException(string.Format(Constantes.ERRO_MODULO_REGISTRADO_CACHE, modulo.Nome, modulo.Id));
                }
                _ = Cache.TryAdd(t.FullName, modulo.Id);
            }

            if (Modulos.ContainsKey(modulo.Id))
            {
                throw new ModuloRegistradoException(string.Format(Constantes.ERRO_MODULO_REGISTRADO, modulo.Nome, modulo.Id));
            }

            IModuloTipo info = new ModuloTipo(modulo);
            _ = Modulos.TryAdd(modulo.Id, info);

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
            if (Modulos != null)
            {
                foreach (KeyValuePair<string, IModuloTipo> modulo in Modulos)
                {
                    if (!modulo.Value.Elimindado)
                    {
                        yield return modulo.Value.Modulo;
                    }
#if DEBUG
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"{modulo.Value} elimindado");
                    }
#endif
                }
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

            RemoverTodos();
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            Modulos.Clear();
            Cache.Clear();
            _atual = null;
            base.Dispose(disposing);

        }
        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.ToString());
            _ = stringBuilder.Append("Ultima atualização: ").Append(UltimaAtualizacao).AppendLine();
            _ = stringBuilder.Append("Modulos inicializados: ").Append(ModulosInicializados).AppendLine();
            return stringBuilder.ToString();

        }


    }
}
