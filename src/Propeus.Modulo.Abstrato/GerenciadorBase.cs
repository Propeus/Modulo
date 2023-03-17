using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Exceptions;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Proveders;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public abstract class GerenciadorBase : ModeloBase, IGerenciador, IGerenciadorDiagnostico
    {
        ///<inheritdoc/>
        public abstract DateTime DataInicio { get; }
        ///<inheritdoc/>
        public virtual DateTime UltimaAtualizacao { get; } = DateTime.Now;
        ///<inheritdoc/>
        public virtual int ModulosInicializados => InstanciaProvider.Provider.Get().Count();

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
        public abstract T Criar<T>() where T : IModulo;
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
        public virtual IModulo Criar(Type modulo)
        {
            if (InstanciaProvider.Provider.HasCache(modulo.Name))
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
                    IModuloTipo gen = InstanciaProvider.Provider.Get().FirstOrDefault(x => x.Modulo is IGerenciador);
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

            return InstanciaProvider.Provider.Create(modulo, arr);

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
        ///                ICalculadoraModuloContrato modulo =  (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato");
        ///                Console.WriteLine(modulo.Calcular(1,1));
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public virtual IModulo Criar(string nomeModulo)
        {
            Type result = TypeProvider.Provider.Get(nomeModulo);
            return result == null
                ? throw new TipoModuloNaoEncontradoException(string.Format(Constantes.ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : Criar(result);
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
        public virtual void Remover(string id)
        {
            if (!InstanciaProvider.Provider.RemoveById(id))
            {
                throw new ModuloNaoEncontradoException("Modulo nao encontrdo");
            }
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
        public virtual void Remover<T>(T modulo) where T : IModulo
        {
            InstanciaProvider.Provider.Remove(modulo);
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
        public virtual void RemoverTodos()
        {
            InstanciaProvider.Provider.Flush();
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
        public abstract T Obter<T>() where T : IModulo;
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
        public virtual IModulo Obter(Type modulo)
        {
            return InstanciaProvider.Provider.GetByName(modulo.Name).Modulo;
        }
        ///<inheritdoc/>
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
        public virtual IModulo Obter(string id)
        {
            var info = InstanciaProvider.Provider.GetById(id);
            return info.Elimindado ? throw new ModuloDescartadoException(string.Format(Constantes.ERRO_MODULO_ID_DESCARTADO, id)) : info.Modulo;
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
        public abstract bool Existe(IModulo modulo);
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
        public virtual bool Existe(Type modulo)
        {
            return InstanciaProvider.Provider.HasByName(modulo.Name);
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
        public virtual bool Existe(string id)
        {
            return InstanciaProvider.Provider.HasById(id);
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
        ///                Console.WriteLine(gerenciador.Reciclar&lt;ICalculadoraModuloContrato&gt;());
        ///            }
        ///        }
        ///    }
        ///}
        ///</code>
        ///</example>
        public abstract T Reciclar<T>(T modulo) where T : IModulo;
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
        public virtual IModulo Reciclar(string id)
        {
            if (InstanciaProvider.Provider.HasById(id))
            {
                var moduloTipo = InstanciaProvider.Provider.GetById(id).TipoModulo;
                InstanciaProvider.Provider.RemoveById(id);
                return Criar(moduloTipo);
            }
            else
            {
                throw new ModuloNaoEncontradoException(Constantes.ERRO_MODULO_NEW_REINICIAR);
            }
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
        public virtual IEnumerable<IModulo> Listar()
        {
            return InstanciaProvider.Provider.Get().Select(x => x.Modulo);
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
        public abstract Task ManterVivoAsync();

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InstanciaProvider.Provider.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
