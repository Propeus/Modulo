using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Threading;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Helpers;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Thread;

using static Propeus.Modulo.Compartilhado.Constantes;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Controlador de modulos
    /// </summary>
    public sealed class Gerenciador : BaseModelo, IGerenciador, IGerenciadorRegistro, IGerenciadorInformacao, IGerenciadorDiagnostico
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
        private static Gerenciador? _atual;

        public DateTime DataInicio { get; }

        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        public int ModulosInicializados => Modulos.Count;

        private TaskJob Workers { get; set; }
        /// <summary>
        /// Dicionario composto por ID do modulo e instancia do tipo do modulo
        /// </summary>
        private ConcurrentDictionary<string, IModuloTipo> Modulos { get; } = new ConcurrentDictionary<string, IModuloTipo>();
        /// <summary>
        /// Dicionario composto por ID do modulo e objetos para instancia do modulo
        /// </summary>
        private ConcurrentDictionary<string, object[]> ArgumentosModulo { get; } = new ConcurrentDictionary<string, object[]>();

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
        public TimeSpan TempoExecucao => DateTime.Now - DataInicio;


        ///<inheritdoc/>
        public IModulo Criar(string nomeModulo)
        {
            IEnumerable<Type> result = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName == nomeModulo ^ t.Name == nomeModulo);
            return !result.Any()
                ? throw new Exception(string.Format(ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : result.Count() > 1
                ? throw new IndexOutOfRangeException(string.Format(ERRO_TIPO_AMBIGUO, nomeModulo))
                : Criar(result.First());
        }

        ///<inheritdoc/>
        public T Criar<T>() where T : IModulo
        {
            return Criar(typeof(T)).To<T>();
        }

        ///<inheritdoc/>
        public IModulo Criar(Type modulo)
        {

            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            if (modulo.IsVoid())
            {
                throw new InvalidCastException(ERRO_TIPO_VOID);
            }


            if (modulo.IsInterface)
            {
                ModuloContratoAttribute attr = modulo.ObterModuloContratoAtributo();

                if (attr is null)
                {
                    throw new InvalidOperationException(ERRO_TIPO_NAO_MARCADO);
                }


                modulo = attr.Tipo ?? attr.Nome.ObterTipo();
                if (modulo is null)
                {
                    throw new DllNotFoundException(string.Format(ERRO_MODULO_NAO_ENCONTRADO, attr.Nome));
                }

            }

            if (modulo.IsClass)
            {
                if (!modulo.Is<IModulo>())
                {
                    throw new InvalidCastException(ERRO_TIPO_NAO_HERDADO);
                }

                if (modulo.ObterModuloAtributo() is null)
                {
                    throw new ArgumentException(ERRO_TIPO_NAO_MARCADO);
                }
            }
            else
            {
                throw new InvalidOperationException(ERRO_TIPO_INVALIDO); //Um modulo deve ser uma interface ou classe
            }


            if (Cache.ContainsKey(modulo.FullName))
            {
                throw new ArgumentException(ERRO_MODULO_INSTANCIA_UNICA);
            }




            ConstructorInfo? ctor = modulo.GetConstructors().MaxBy(x => x.GetParameters().Length);
            if (ctor is null)
            {
                throw new InvalidOperationException(ERRO_CONSTRUTOR_NAO_ENCONTRADO);
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
                        //TODO: Deve criar um novo caso nao seja singleton
                        arr[i] = Obter(paramCtor[i].ParameterType).To(paramCtor[i].ParameterType);
                    }
                }
                else if (paramCtor[i].ParameterType.Is<IGerenciador>())
                {
                    var gen = Modulos.FirstOrDefault(x => x.Value.Modulo is IGerenciador).Value;
                    if (gen is null)
                    {
                        arr[i] = Atual;
                    }
                    else
                    {
                        arr[i] = gen;
                    }
                }
                else
                {
                    if (paramCtor[i].IsOptional)
                        continue;

                    throw new InvalidCastException($"O tipo '{paramCtor[i].ParameterType.Name}' nao e um Modulo, Contrato ou Gerenciador");
                }
            }






            IModulo mAux = Activator.CreateInstance(modulo).As<IModulo>();
            return mAux;

        }


        ///<inheritdoc/>
        public void Remover<T>(T modulo) where T : IModulo
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo), string.Format(ARGUMENTO_NULO, nameof(modulo)));
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
        public void Remover(string id)
        {
            Remover(Obter(id));
        }

        ///<inheritdoc/>
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
        public IModuloTipo ObterInfo<T>() where T : IModulo
        {
            return ObterInfo(typeof(T));
        }

        ///<inheritdoc/>
        public IModuloTipo ObterInfo(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo), string.Format(ARGUMENTO_NULO, nameof(modulo)));
            }

            if (modulo.IsVoid())
            {
                throw new InvalidCastException(ERRO_TIPO_VOID);
            }

            IEnumerable<IModuloTipo>? info = null;
            if (modulo.IsInterface)
            {
                ModuloContratoAttribute contrato = modulo.ObterModuloContratoAtributo();
                modulo = contrato.Tipo ?? contrato.Nome.ObterTipo();
            }


            if (modulo is null)
            {
                throw new NullReferenceException(string.Format(ERRO_MODULO_NAO_ENCONTRADO, modulo.FullName));
            }


            if (modulo.IsClass)
            {
                if (Cache.TryGetValue(modulo.FullName, out string idOuter))
                {
                    if (Modulos.TryGetValue(idOuter, out IModuloTipo infoOuter))
                    {
                        info = new IModuloTipo[] { infoOuter };
                    }
                }

                info ??= Modulos
                           .Select(x => x.Value)
                           .Where(x => x.TipoModulo.FullName == modulo.Name
                           || x.TipoModulo.Name == modulo.Name);
            }
            else
            {
                throw new InvalidOperationException(ERRO_TIPO_INVALIDO); //Um modulo deve ser uma interface ou classe
            }

            if (info is null || !info.Any())
            {
                throw new ArgumentException(string.Format(ERRO_MODULO_NAO_ENCONTRADO, modulo.Name));
            }

            return info.First(m => !m.Elimindado);
        }

        ///<inheritdoc/>
        public IModuloTipo ObterInfo(string id)
        {
            if (!Existe(id))
            {
                throw new ArgumentException(string.Format(ERRO_MODULO_ID_NAO_ENCONTRADO, id), nameof(id));
            }

            return Modulos[id];
        }

        ///<inheritdoc/>
        public T Obter<T>() where T : IModulo
        {
            return Obter(typeof(T)).To<T>();
        }

        ///<inheritdoc/>
        public IModulo Obter(Type modulo)
        {
            var result = ObterInfo(modulo);
            return result.Elimindado ? throw new InvalidProgramException(string.Format(ERRO_MODULO_ID_DESCARTADO, result.Nome)) : result.Modulo;
        }

        ///<inheritdoc/>
        public IModulo Obter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), string.Format(ERRO_ARGUMENTO_NULO_OU_VAZIO, nameof(id)));
            }

            if (!Existe(id))
            {
                throw new ArgumentException(string.Format(ERRO_MODULO_ID_NAO_ENCONTRADO, id), nameof(id));
            }

            IModuloTipo info = Modulos[id];


            return info.Elimindado ? throw new InvalidProgramException(string.Format(ERRO_MODULO_ID_DESCARTADO, id)) : info.Modulo;
        }


        ///<inheritdoc/>
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
                    throw new InvalidProgramException(ERRO_MODULO_CONTRATO_NAO_ENCONTRADO);
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

            }
            else
            {
                throw new InvalidOperationException(ERRO_TIPO_INVALIDO); //Um modulo deve ser uma interface ou classe
            }

            return result;
        }

        ///<inheritdoc/>
        public bool Existe(IModulo modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : Existe(modulo.Id);
        }

        ///<inheritdoc/>
        public bool Existe(string id)
        {
            return string.IsNullOrEmpty(id) ? throw new ArgumentNullException(nameof(id)) : Modulos.ContainsKey(id);
        }


        ///<inheritdoc/>
        public T Reiniciar<T>(T modulo) where T : IModulo
        {
            if (ArgumentosModulo.TryGetValue(modulo.Id, out object[] args))
            {
                Remover(modulo);
                return Criar<T>();
            }
            else
            {
                throw new InvalidOperationException(ERRO_MODULO_NEW_REINICIAR);
            }

        }

        ///<inheritdoc/>
        public IModulo Reiniciar(string id)
        {
            IModulo modulo = Obter(id);
            Remover(id);
            return Criar(modulo.GetType());
        }


        ///<inheritdoc/>
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
                    throw new ArgumentException(string.Format(ERRO_MODULO_REGISTRADO_CACHE, modulo.Nome, modulo.Id));
                }
                _ = Cache.TryAdd(t.FullName, modulo.Id);
            }

            if (Modulos.ContainsKey(modulo.Id))
            {
                throw new ArgumentException(string.Format(ERRO_MODULO_REGISTRADO, modulo.Nome, modulo.Id));
            }

            IModuloTipo info = new ModuloTipo(modulo);
            _ = Modulos.TryAdd(modulo.Id, info);

        }

        ///<inheritdoc/>
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
            StringBuilder stringBuilder = new(base.ToString());
            _ = stringBuilder.Append("Ultima atualização: ").Append(UltimaAtualizacao).AppendLine();
            _ = stringBuilder.Append("Modulos inicializados: ").Append(ModulosInicializados).AppendLine();
            return stringBuilder.ToString();

        }


    }
}
