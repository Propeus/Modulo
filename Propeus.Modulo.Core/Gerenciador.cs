using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Util;

using static Propeus.Modulo.Abstrato.Constantes;

namespace Propeus.Modulo.Core
{
    public sealed class Gerenciador : BaseModelo, IGerenciador, IGerenciadorRegistro
    {
        private Gerenciador()
        {
            DataInicio = DateTime.Now;

            LimpezaAutomatica();
            Console.CancelKeyPress += Console_CancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            ModulosValidos = Array.Empty<string>();
            ModulosIgnorados = Array.Empty<string>();
        }

        private readonly CancellationTokenSource _cancellationToken = new();
        private static Gerenciador? _atual;

        private DateTime DataInicio { get; }

        private DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        private Task? LimpezaListaTask { get; set; } = null;
        /// <summary>
        /// Dicionario composto por ID do modulo e instancia do tipo do modulo
        /// </summary>
        private ConcurrentDictionary<string, IModuloTipo> Modulos { get; } = new ConcurrentDictionary<string, IModuloTipo>();
        /// <summary>
        /// Dicionario composto por ID do modulo e objetos para instancia do modulo
        /// </summary>
        private ConcurrentDictionary<string, object[]> ArgumentosModulo { get; } = new ConcurrentDictionary<string, object[]>();

        private ConcurrentDictionary<string, string> Cache { get; } = new ConcurrentDictionary<string, string>();


        public static IGerenciador Atual
        {
            get
            {
                if (_atual is null || _atual.Disposed)
                {
                    _atual = new Gerenciador();
                }

                return _atual;
            }
        }

        public Task LimpezaAutomaticaTask { get; private set; }

        public IEnumerable<string> ModulosValidos { get; }

        public IEnumerable<string> ModulosIgnorados { get; }

        public TimeSpan TempoExecucao => DateTime.Now - DataInicio;


        public IModulo Criar(string nomeModulo, params object[] argumentos)
        {
            IEnumerable<Type> result = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName == nomeModulo ^ t.Name == nomeModulo);
            return !result.Any()
                ? throw new Exception(string.Format(ERRO_NOME_MODULO_NAO_ENCONTRADO, nomeModulo))
                : result.Count() > 1
                ? throw new IndexOutOfRangeException(string.Format(ERRO_TIPO_AMBIGUO, nomeModulo))
                : Criar(result.First(), argumentos);
        }

        public T Criar<T>(params object[] argumentos) where T : IModulo
        {
            return Criar(typeof(T), argumentos).To<T>();
        }

        public IModulo Criar(Type modulo, params object[] argumentos)
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

                modulo = modulo.ObterTipoPorModuloContratoAtributo();
                if (modulo is null)
                {
                    throw new DllNotFoundException(string.Format(ERRO_MODULO_NAO_ENCONTRADO, attr.Nome));
                }

            }

            if (modulo.IsClass)
            {
                if (modulo.Is<IModulo>().Not())
                {
                    throw new InvalidCastException(ERRO_TIPO_NAO_HERDADO);
                }

                if (modulo.ObterModuloAtributo().IsNull())
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

            ConstructorInfo? ctor = null;
            if (argumentos.Any())
            {
                ctor = modulo.GetConstructors().OrderByDescending(x => x.GetParameters().Length == argumentos.Length).FirstOrDefault();
                if (ctor is null)
                {
                    throw new InvalidOperationException(ERRO_CONSTRUTOR_NAO_ENCONTRADO);
                }
                ParameterInfo[] paramst = ctor.GetParameters();
                object[] arr = new object[paramst.Length];
                for (int i = 0; i < paramst.Length; i++)
                {
                    //Melhorar a busca e atribuição de parametros
                    arr[i] = paramst[i].ParameterType.Is<IGerenciador>()
                        ? !argumentos[i].Herdado<IGerenciador>()
                            ? throw new ArgumentException(string.Format(ERRO_ARGUMENTO_TIPO_ESPERADO, paramst[i].Name, paramst[i].ParameterType.Name, argumentos[i].GetType().Name))
                            : argumentos[i]
                        : paramst[i].DefaultValue.IsNotNull() ? argumentos.Length < i ? paramst[i].DefaultValue : argumentos[i] : argumentos[i];

                }
                argumentos = arr;

            }
            else
            {
                ctor = modulo.GetConstructors().OrderBy(x => x.GetParameters()).FirstOrDefault();
                if (ctor is null)
                {
                    throw new InvalidOperationException(ERRO_CONSTRUTOR_NAO_ENCONTRADO);
                }

                ParameterInfo[] paramst = ctor.GetParameters();

                if (paramst.Length == 0)
                {
                    throw new InvalidOperationException(ERRO_CONSTRUTOR_IGERENCIADOR_NAO_ENCONTRADO);
                }

                object[] arr = new object[paramst.Length];
                for (int i = 0; i < paramst.Length; i++)
                {

                    arr[i] = paramst[i].ParameterType.Is<IGerenciador>()
                        ? this.As<IGerenciador>()
                        : paramst[i].DefaultValue.IsNotNull() ? paramst[i].DefaultValue : paramst[i].ParameterType.Default();
                }
                argumentos = arr;
            }

            IModulo mAux = Activator.CreateInstance(modulo, argumentos).As<IModulo>();
            _ = ArgumentosModulo.TryAdd(mAux.Id, argumentos);
            return mAux;

        }


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

        public void Remover(string id)
        {
            Remover(Obter(id));
        }

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


        public T Obter<T>() where T : IModulo
        {
            return Obter(typeof(T)).To<T>();
        }

        public IModulo Obter(Type modulo)
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
                modulo = modulo.ObterTipoPorModuloContratoAtributo();
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
                           || x.TipoModuloDinamico?.FullName == modulo.Name
                           || x.TipoModulo.Name == modulo.Name
                           || x.TipoModuloDinamico?.Name == modulo.Name);
            }
            else
            {
                throw new InvalidOperationException(ERRO_TIPO_INVALIDO); //Um modulo deve ser uma interface ou classe
            }

            if (info is null || !info.Any())
            {
                throw new ArgumentException(string.Format(ERRO_MODULO_NAO_ENCONTRADO, modulo.Name));
            }

            IModuloTipo result = info.First(m => !m.Elimindado);
            return result.Elimindado ? throw new InvalidProgramException(string.Format(ERRO_MODULO_ID_DESCARTADO, result.Nome)) : result.Modulo;
        }

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


        public bool Existe(Type type)
        {
            if (type.IsNull())
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

        public bool Existe(IModulo modulo)
        {
            return modulo is null ? throw new ArgumentNullException(nameof(modulo)) : Existe(modulo.Id);
        }

        public bool Existe(string id)
        {
            return string.IsNullOrEmpty(id) ? throw new ArgumentNullException(nameof(id)) : Modulos.ContainsKey(id);
        }


        public T Reiniciar<T>(T modulo) where T : IModulo
        {
            if (ArgumentosModulo.TryGetValue(modulo.Id, out object[] args))
            {
                Remover(modulo);
                return Criar<T>(args);
            }
            else
            {
                throw new InvalidOperationException(ERRO_MODULO_NEW_REINICIAR);
            }

        }

        public IModulo Reiniciar(string id)
        {


            if (ArgumentosModulo.TryGetValue(id, out object[] args))
            {
                IModulo modulo = Obter(id);
                Remover(id);
                return Criar(modulo.GetType(), args);
            }
            else
            {
                throw new InvalidOperationException(ERRO_MODULO_NEW_REINICIAR);
            }
        }


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

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Dispose();
            e.Cancel = true;
        }

        private void LimpezaAutomatica()
        {
            LimpezaAutomaticaTask = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false);

                    await LimpezaLista().ConfigureAwait(true);

                    UltimaAtualizacao = DateTime.Now;
                }
            }, _cancellationToken.Token);

        }
        private Task LimpezaLista()
        {
            LimpezaListaTask = Task.Run(() =>
            {
                List<string> keys = Modulos.Where(x => x.Value.Elimindado).Select(x => x.Key).ToList();
                foreach (string key in keys)
                {
                    _ = Cache.TryRemove(key, out _);
                    _ = Modulos.TryRemove(key, out _);
                }

            }, _cancellationToken.Token);
            return LimpezaListaTask;
        }
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Atual.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                RemoverTodos();
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
                Modulos.Clear();
                Cache.Clear();
                _atual = null;
                base.Dispose(disposing);
            }
            else
            {
                throw new ObjectDisposedException(ERRO_GERENCIADOR_DESATIVADO);
            }
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());


            _ = stringBuilder.Append("---").Append(Nome).Append("---").AppendLine();

            _ = stringBuilder.Append("Ultima atualização: ").Append(UltimaAtualizacao).AppendLine();

            _ = stringBuilder.Append("Quantidade de modulos mapeados: ").Append(ModulosValidos.Count()).AppendLine();

            _ = stringBuilder.Append("---").Append(Nome).Append("---").AppendLine();


            return stringBuilder.ToString();

        }


    }
}
