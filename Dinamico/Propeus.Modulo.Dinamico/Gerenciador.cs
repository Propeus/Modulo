using Propeus.Modulo.Dinamico.Properties;
using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Delegates;
using Propeus.Modulo.Modelos.Helpers.Attributes;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Modelos.Interfaces.Registro;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Thread;
using Propeus.Modulo.Util.Strings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Propeus.Modulo.Modelos.Constantes.Helper;

namespace Propeus.Modulo.Dinamico
{
    [Modulo]
    public class Gerenciador : ModuloBase, IGerenciador, IGerenciadorRegistro
    {
        public Gerenciador(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
            dataInicio = DateTime.Now;
            ConfiguracaoInicial();

            autoResetEvent = new AutoResetEvent(false);
            timer = new Timer(AtualizarModulos, autoResetEvent, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_tempoAtualziacaoModulo));
            autoResetEvent.WaitOne();
     
        }

        private void AtualizarModulos(object state)
        {
            AutoResetEvent gen = (AutoResetEvent)state;

            MapearModulos().Wait();
            MapearModulosAutoInicializavel().Wait();

            gen.Set();
        }

        public TimeSpan TempoExecucao => DateTime.Now - dataInicio;
        private bool modulosCarregados = false;
        private bool emAtualizacao = false;
        private int quantidadeModulosDiretorio = 0;

        private readonly DateTime dataInicio;
        private readonly ConcurrentDictionary<string, IModuloTipo> Modulos = new ConcurrentDictionary<string, IModuloTipo>();
        private readonly ConcurrentDictionary<Type, bool> ModulosAutoinicializaveis = new ConcurrentDictionary<Type, bool>();
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        private readonly double _tempoAtualziacaoModulo = 5;
        private readonly Timer timer;
        private readonly AutoResetEvent autoResetEvent;

        private Dictionary<string, List<string>> ModulosDicionario;

        private TaskFactory taskFactory;
        private LimitedConcurrencyLevelTaskScheduler scheduler;
        private List<Task> tarefas;

        public event Evento OnEvento;

        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();




        public T Criar<T>(params object[] args) where T : IModulo
        {
            return Criar(typeof(T), args).As<T>();
        }
        public IModulo Criar(string nomeModulo, params object[] args)
        {
            IEnumerable<KeyValuePair<string, IModuloTipo>> query = ObterTipoPorNome(nomeModulo);
            KeyValuePair<string, IModuloTipo> info = query.First();

            return Criar(info.Value.TipoModuloDinamico ?? info.Value.TipoModulo, args);
        }
        public IModulo Criar(Type modulo, params object[] args)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            IModulo result;

            //Obtem o tipo do modulo
            var nModulo = ObterTipoPorInterface(modulo);
            //Cria a instancia do tipo (Gerenciador base)
            result = Gerenciador.Criar(nModulo, args);

            //Verifica se existe algum metodo com o nome da constante `MetodoInstancia`
            //Metodo destinado a receber instancias de dos parametros solicitados
            if (result.ExisteMetodo(MetodoInstancia))
            {
                Type[] @params = result.ObterParametrosMetodo(MetodoInstancia);
                List<object> lParams = new List<object>();
                foreach (Type item in @params)
                {
                    //Verifica se algum parametro do modulo precisa do gerenciador atual
                    if (item.Is<IGerenciador>())
                    {
                        lParams.Add(this);
                    }

                    //Verifica se algum parametro e uma interface de contrato ou modulo
                    if (item.Is<IModulo>() || item.Herdado<IModulo>())
                    {
                        //Cria a instancia do tipo e injeta no parametro (Chamada recursiva)
                        lParams.Add(Criar(item));
                    }

                    //Caso seja tipos primitivos? Adicinar Validacao ou setar tudo nulo?
                }
                result.InvocarMetodo(MetodoInstancia, lParams.ToArray());
            }

            //Verifica se existe algum metodo com o nome da constante `MetodoConfiguracao`
            //Metodo destinado a configurar as instancias de dos parametros solicitados
            if (result.ExisteMetodo(MetodoConfiguracao))
            {
                Type[] @params = result.ObterParametrosMetodo(MetodoConfiguracao);
                List<object> lParams = new List<object>();
                foreach (Type item in @params)
                {
                    //Verifica se algum parametro do modulo precisa do gerenciador atual
                    if (item.Is<IGerenciador>())
                    {
                        lParams.Add(this);
                    }

                    //Verifica se algum parametro e uma interface de contrato ou modulo
                    if (item.Is<IModulo>() || item.Herdado<IModulo>())
                    {
                        //Cria a instancia do tipo e injeta no parametro (Chamada recursiva)
                        lParams.Add(Criar(item));
                    }

                }
                result.InvocarMetodo(MetodoConfiguracao, lParams.ToArray());
            }
            return result;
        }


        public void Remover<T>(T modulo) where T : IModulo
        {
            Gerenciador.Remover(modulo);
        }
        public void Remover(string id)
        {
            if (Gerenciador.Existe(id))
            {
                Gerenciador.Remover(id);
            }
        }
        public void RemoverTodos()
        {
            foreach (KeyValuePair<string, IModuloTipo> item in Modulos)
            {
                Remover(item.Key);
            }
        }


        public T Reiniciar<T>(T modulo) where T : IModulo
        {
            T nModulo = Gerenciador.Reiniciar(modulo);
            return nModulo;
        }
        public IModulo Reiniciar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ERRO_ID_NULO_OU_VAZIO, nameof(id)), nameof(id));
            }

            if (Modulos.ContainsKey(id))
            {
                IModulo nModulo = Gerenciador.Reiniciar(id);
                return nModulo;
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ERRO_ID_NAO_ENCONTRADO, nameof(id)), nameof(id));
            }
        }


        public T Obter<T>() where T : IModulo
        {
            Type type = typeof(T);
            return Obter(type).To<T>();
        }
        public IModulo Obter(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var nModulo = ObterTipoPorInterface(type);

            IModulo modulo = Gerenciador.Obter(nModulo);
            return modulo;
        }
        public IModulo Obter(string id)
        {
            IModulo modulo = Gerenciador.Obter(id);
            return modulo;
        }

        public bool Existe(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var modulo = ObterTipoPorInterface(type);
            return Gerenciador.Existe(modulo);
        }
        public bool Existe(IModulo modulo)
        {
            return Gerenciador.Existe(modulo);
        }
        public bool Existe(string id)
        {
            return Gerenciador.Existe(id);
        }

        public IEnumerable<IModulo> Listar()
        {
            foreach (KeyValuePair<string, IModuloTipo> moduloWr in Modulos)
            {
                if (!moduloWr.Value.Elimindado)
                {
                    yield return moduloWr.Value.Modulo;
                }
            }
        }

        public void Registrar(IModulo modulo)
        {
            if (Gerenciador is IGerenciadorRegistro)
            {
                (Gerenciador as IGerenciadorRegistro).Registrar(modulo);
            }
            else
            {
                throw new InvalidCastException(string.Format(Resources.Culture, Resources.ERRO_GERENCIADOR_NAO_POSSUI_INTERFACE, Gerenciador.Nome, nameof(IGerenciadorRegistro)));
            }
        }

    

        public async Task ManterVivoAsync()
        {
            await Gerenciador.ManterVivoAsync().ConfigureAwait(true);
        }

        public async Task MapearModulos()
        {
            int quantidadeAtual = Directory.GetFiles(DiretorioModulo, Resources.EXT_DLL).Length;
            await MapearModulos(quantidadeModulosDiretorio != quantidadeAtual).ConfigureAwait(true);
            quantidadeModulosDiretorio = quantidadeAtual;
        }

        private Task MapearModulos(bool mapearDiretorio)
        {

            if (emAtualizacao)
            {
                OnEvento?.Invoke(new[] { Resources.INFO_MAPEAMENTO_EM_ANDAMENTO });
                return Task.CompletedTask;
            }

            emAtualizacao = true;
            IEnumerable<string> libs = MapearDiretorios(mapearDiretorio);

            Regras.ModuloComAtributoRegra regraAtributo = new Regras.ModuloComAtributoRegra();
            Regras.ModuloIgnorarRegra moduloIgnorarRegra = new Regras.ModuloIgnorarRegra();

            for (int cont = 0; cont < libs.Count(); cont++)
            {
                CriarTaskScheduler((lib) =>
                {
                    string path = (string)lib;

                    ModuloBinario moduloBin = new ModuloBinario(path);

                    if (ModuloInformacao.PossuiModuloValido(moduloBin, regraAtributo, moduloIgnorarRegra))
                    {
                        ModuloInformacao moduloInfo = new ModuloInformacao(moduloBin);
                        foreach (var modulotipo in moduloInfo.Modulos)
                        {
                            var aux = modulotipo.Value;

                            Modulos.AddOrUpdate(aux.Id, aux, (chave, antigoValor) =>
                            {
                                antigoValor.Dispose();
                                return aux;
                            });
                        }

                        if (!ModulosDicionario[Resources.DinamicoDiretorioModulosValidos].Contains(path))
                        {
                            ModulosDicionario[Resources.DinamicoDiretorioModulosValidos].Add(path);
                        }
                    }
                    else
                    {
                        if (!ModulosDicionario[Resources.DinamicoDiretorioModulosIgnorados].Contains(path))
                        {
                            ModulosDicionario[Resources.DinamicoDiretorioModulosIgnorados].Add(path);
                        }
                    }

                }, libs.ElementAt(cont));


            }

            Task.WaitAll(tarefas.ToArray());

            modulosCarregados = true;
            SalvarConfiguracoes();
            emAtualizacao = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Mapeia todas as DLLs indentificando quais são modulos validos e invalidos
        /// </summary>
        /// <param name="mapearDiretorio"></param>
        /// <returns></returns>
        private IEnumerable<string> MapearDiretorios(bool mapearDiretorio)
        {
            IEnumerable<string> libs = Array.Empty<string>();
            if (mapearDiretorio)
            {
                libs = Directory.GetFiles(DiretorioModulo, Resources.EXT_DLL);
            }

            libs = libs.Union(ModulosDicionario[Resources.DinamicoDiretorioModulosValidos]);

            if (ModulosDicionario[Resources.DinamicoDiretorioModulosIgnorados].Any())
            {
                libs = libs.Except(ModulosDicionario[Resources.DinamicoDiretorioModulosIgnorados]);
            }

            return libs;
        }

        private Task MapearModulosAutoInicializavel()
        {
            foreach (var modulo in Modulos)
            {
                if (modulo.Value.TipoModulo.PossuiAtributo<ModuloAutoInicializavelAttribute>())
                {
                    if (ModulosAutoinicializaveis.TryGetValue(modulo.Value.TipoModulo, out bool inicializado))
                    {
                        if (!inicializado)
                        {
                            Criar(modulo.Value.TipoModulo);
                            ModulosAutoinicializaveis[modulo.Value.TipoModulo] = true;
                        }
                    }
                    else
                    {
                        Criar(modulo.Value.TipoModulo);
                        ModulosAutoinicializaveis.TryAdd(modulo.Value.TipoModulo, true);
                    }
                }
            }

            return Task.CompletedTask;
        }


        private void ConfiguracaoInicial()
        {
            Gerenciador th = this;
            ModulosDicionario = new Dictionary<string, List<string>>() { { Resources.DinamicoDiretorioModulosValidos, new List<string>() }, { Resources.DinamicoDiretorioModulosIgnorados, new List<string>() } };
            string config = nameof(th) + Resources.EXT_INDICE;
            if (!File.Exists(config))
            {
                OnEvento?.Invoke(new object[] { Resources.INFO_INDICE_CRIADO });
                SalvarConfiguracoes();
            }
            else
            {
                OnEvento?.Invoke(new object[] { Resources.INFO_INDICE_LIDO });
                ModulosDicionario = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(File.ReadAllText(config));
            }

        }

        private void SalvarConfiguracoes()
        {
            string th = GetType().FullName;
            using FileStream fileStream = new FileStream(th + Resources.EXT_INDICE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(System.Text.Json.JsonSerializer.Serialize(ModulosDicionario).ToArrayByte());
            fileStream.Write(span);
            fileStream.Close();
        }

        private void CriarTaskScheduler(Action<object> action, object estado)
        {
            if (scheduler is null)
            {
                scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            }

            if (taskFactory is null)
            {
                taskFactory = new TaskFactory(scheduler);
            }

            if (tarefas is null)
            {
                tarefas = new List<Task>();
            }

            tarefas.Add(taskFactory.StartNew(action, estado, _cancellationToken.Token, TaskCreationOptions.DenyChildAttach, scheduler));
        }

        private IEnumerable<KeyValuePair<string, IModuloTipo>> ObterTipoPorNome(string nomeModulo)
        {
            IEnumerable<KeyValuePair<string, IModuloTipo>> query = Modulos.Where(y => y.Key == nomeModulo || y.Value.TipoModulo.FullName == nomeModulo);
            if (query.Count() > 1)
            {
                throw new ArgumentOutOfRangeException(string.Format(Resources.Culture, Resources.ERRO_AMBIGUIDADE_NOME, nomeModulo));
            }

            if (!query.Any())
            {
                throw new DllNotFoundException(string.Format(Resources.Culture, Resources.ERRO_MODULO_NAO_ENCONTRADO, nomeModulo));
            }

            return query;
        }
        private Type ObterTipoPorInterface(Type modulo)
        {
            //Verifica se o tipo e uma classe e se e herdado da interface IModulo
            if (modulo.IsClass && modulo.Herdado<IModulo>())
            {
                return modulo;
            }

            //Se o tipo nao for uma interface herdado de IModulo e nao possui o atributo de Interface de contrato 
            //Lanca excecao informando que a interface nao e valida
            if (!modulo.IsInterface || !modulo.Herdado<IModulo>() || !modulo.PossuiAtributo<ModuloContratoAttribute>())
            {
                throw new InvalidCastException(Resources.ERRO_TIPO_MODULO_INVALIDO);
            }

            var contrato = modulo.ObterModuloContratoAtributo();
            var nomeModulo = contrato.Nome;

            //Obtem os modulos que possui o mesmo nome da interface de contrato
             var query = Modulos.Where(modulo => modulo.Value.TipoModuloDinamico?.FullName == nomeModulo
                                                || modulo.Value.TipoModuloDinamico?.Name == nomeModulo
                                                || modulo.Value.TipoModulo.FullName == nomeModulo
                                                || modulo.Value.TipoModulo.Name == nomeModulo);
            //Caso encontre varios modulos com o mesmo nome
            if (query.Count() > 1)
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ERRO_AMBIGUIDADE_NOME, nomeModulo));
            }
            //Caso nao encontre nada
            if (!query.Any())
            {
                throw new InvalidOperationException(string.Format(Resources.Culture, Resources.ERRO_MODULO_NAO_ENCONTRADO, nomeModulo));
            }

            //Obtem o metedata do modulo
            IModuloTipo moduloTipo = query.First().Value;
            //Adiciona a interface de contrato na lista de contratos do modulo
            moduloTipo.AdicionarContrato(modulo);

            //Obtem o tipo original ou o tipo dinamico
            var tipo = moduloTipo.TipoModuloDinamico ?? moduloTipo.TipoModulo;

            if (tipo == null)
            {
                throw new DllNotFoundException(string.Format(Resources.Culture, Resources.ERRO_MODULO_NAO_ENCONTRADO, nomeModulo));
            }

            return tipo;
        }

        protected override void Dispose(bool disposing)
        {
            timer.Dispose();
            autoResetEvent.Dispose();
            _cancellationToken.Cancel();
            SalvarConfiguracoes();
            RemoverTodos();
            base.Dispose(disposing);
            _cancellationToken.Dispose();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(base.ToString());

            stringBuilder.Append("---").Append(Nome).Append("---").AppendLine();

            stringBuilder.Append("Data de inicio da execução do Gerenciador: ").Append(dataInicio).AppendLine();
            stringBuilder.Append("Tempo em execução: ").Append(TempoExecucao).AppendLine();

            stringBuilder.Append("Modulos Carregados: ").Append(modulosCarregados ? "Sim" : "Não").AppendLine();
            stringBuilder.Append("Modulos em atualização: ").Append(emAtualizacao ? "Sim" : "Não").AppendLine();
            stringBuilder.Append("Caminho do diretório: ").Append(DiretorioModulo).AppendLine();
            stringBuilder.Append("Quantidade de DLLs no diretório: ").Append(quantidadeModulosDiretorio).AppendLine();
            stringBuilder.Append("Tempo de atualização dos modulos: ").Append(_tempoAtualziacaoModulo).Append(" segundos").AppendLine();

            stringBuilder.Append("---").Append(Nome).Append("---").AppendLine();


            return stringBuilder.ToString();
        }


    }
}
