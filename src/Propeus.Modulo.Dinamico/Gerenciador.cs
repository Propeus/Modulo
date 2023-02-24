using System;
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
using Propeus.Modulo.Dinamico.Properties;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;
using Propeus.Modulo.Util.Thread;

namespace Propeus.Modulo.Dinamico
{
    [Modulo]
    public class Gerenciador : ModuloBase, IGerenciador, IGerenciadorDiagnostico
    {
        public Gerenciador(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
            Binarios = new ConcurrentDictionary<string, IModuloBinario>();
            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            DataInicio = DateTime.Now;
            _scheduler = new TaskJob(10);

            //[POSSIVEL PROBLEMA]Pode acontecer de chamar o moduloBin antes de mapear
            _scheduler.AddTask((cancelationToken) =>
            {
                var arquivos_dll = Directory.GetFiles(DiretorioModulo, Resources.EXT_DLL);
                foreach (var dll in arquivos_dll)
                {
                    var moduloBinario = new ModuloBinario(dll);
                    if (moduloBinario.BinarioValido)
                    {
                        if (!Binarios.ContainsKey(dll))
                        {
                            Binarios.TryAdd(dll, moduloBinario);
                        }
                        else
                        {
                            if (Binarios[dll].Hash != moduloBinario.Hash)
                            {
                                Binarios.TryRemove(dll, out IModuloBinario moduloBin);
                                moduloBin.Dispose();
                                Binarios.TryAdd(dll, moduloBinario);
                            }

                        }
                    }
                    else
                    {
                        moduloBinario.Dispose();
                    }
                }
            }, TimeSpan.FromSeconds(1), "CARREGAR_MODULO_JOB");
            _scheduler.AddTask((cancelationToken) =>
            {
                foreach (var moduloBin in Binarios)
                {

                    var modulos = moduloBin.Value.ModuloInformacao.Assembly.GetTypes().Where(x => x.Herdado<IModulo>() && x.PossuiAtributo<ModuloAttribute>() && x.PossuiAtributo<ModuloAutoInicializavelAttribute>()).ToArray();
                    foreach (var modulo in modulos)
                    {
                        if (!Existe(modulo))
                        {
                            Criar(modulo);
                        }
                        else
                        {
                            string oldVer = (Gerenciador as IGerenciadorInformacao).ObterInfo(modulo).Versao;
                            string newVer = moduloBin.Value.ModuloInformacao.Versao;
                            if (oldVer != newVer)
                            {
                                var iModulo = Obter(modulo);
                                Remover(iModulo);
                                Criar(modulo);

                            }
                        }
                    }
                }
            }, TimeSpan.FromSeconds(1), "AUTO_INICIALIZAR_MODULO_JOB");

        }

        public DateTime DataInicio { get; }

        private TaskJob _scheduler;

        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        public int ModulosInicializados => ((Gerenciador is IGerenciadorDiagnostico) ? (Gerenciador as IGerenciadorDiagnostico).ModulosInicializados : -1);

        private ConcurrentDictionary<string, IModuloBinario> Binarios { get; }
        private Dictionary<string, ILClasseProvider> ModuloProvider { get; }


        public event Evento OnEvento;

        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();


        public T Criar<T>(params object[] args) where T : IModulo
        {
            return Criar(typeof(T), args).As<T>();
        }
        public IModulo Criar(string nomeModulo, params object[] args)
        {
            var binM = Binarios.Single(x => x.Value.ModuloInformacao.PossuiModulo(nomeModulo)).Value;

            return Criar(binM.ModuloInformacao.CarregarTipoModulo(nomeModulo), args);
        }
        public IModulo Criar(Type modulo, params object[] args)
        {
            if (modulo.IsInterface)
            {
                if (modulo.PossuiAtributo<ModuloContratoAttribute>())
                {

                    var nmeTipo = modulo.ObterAtributo<ModuloContratoAttribute>()?.Nome;
                    var infoBin = Binarios.Single(x => x.Value.ModuloInformacao.PossuiModulo(nmeTipo)).Value;
                    var tipoBase = infoBin.ModuloInformacao.CarregarTipoModulo(nmeTipo);



                    IModulo iModulo;
                    if (tipoBase.Herdado(modulo))
                    {
                        iModulo = Gerenciador.Criar(nmeTipo, args);
                    }
                    else
                    {

                        ILClasseProvider provider;
                        if (ModuloProvider.ContainsKey(nmeTipo))
                        {
                            infoBin.ModuloInformacao.AdicionarContrato(nmeTipo, modulo);

                            provider = ModuloProvider[nmeTipo];
                            ModuloProvider[nmeTipo] = provider.NovaVersao(interfaces: infoBin.ModuloInformacao.ObterContratos(nmeTipo).ToArray());
                        }
                        else
                        {
                            var interfaces = tipoBase.ObterInterfaces();
                            foreach (var item in interfaces)
                            {
                                infoBin.ModuloInformacao.AdicionarContrato(nmeTipo, item);
                            }
                            provider = GeradorHelper.ObterModuloGenerico().CriarProxyClasse(tipoBase, infoBin.ModuloInformacao.ObterContratos(nmeTipo).ToArray());
                            ModuloProvider.Add(nmeTipo, provider);
                        }
                        provider.Executar();  //Falta adicionar um atributo moduloBin
                        iModulo = Gerenciador.Criar(provider.ObterTipoGerado(), args);
                        infoBin.ModuloInformacao[nmeTipo] = (Gerenciador as IGerenciadorInformacao).ObterInfo(provider.ObterTipoGerado());
                    }


                    return iModulo;

                }
                else
                {
                    throw new ArgumentException("A interface nao possui o atributo de contrato");
                }
            }
            else
            {
                return Gerenciador.Criar(modulo, args);
            }
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
            Gerenciador.RemoverTodos();
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

            return Gerenciador.Reiniciar(id);


        }


        public T Obter<T>() where T : IModulo
        {
            Type type = typeof(T);
            return Obter(type).To<T>();
        }
        public IModulo Obter(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            return Gerenciador.Obter(modulo);
        }
        public IModulo Obter(string id)
        {
            IModulo modulo = Gerenciador.Obter(id);
            return modulo;
        }

        public bool Existe(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

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
            return Gerenciador.Listar();
        }

        public async Task ManterVivoAsync()
        {
            await Gerenciador.ManterVivoAsync().ConfigureAwait(true);
        }


        protected override void Dispose(bool disposing)
        {
            RemoverTodos();
            base.Dispose(disposing);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new(base.ToString());



            _ = stringBuilder.Append("Data de inicializacao: ").Append(DataInicio).AppendLine();
            _ = stringBuilder.Append("Tempo em execução: ").Append(DateTime.Now - DataInicio).AppendLine();
            _ = stringBuilder.Append("Jobs em execucao: ").Append(_scheduler.EmExecucao).AppendLine();


            //_ = stringBuilder.Append("ModuloInformacao Carregados: ").Append(modulosCarregados ? "Sim" : "Não").AppendLine();
            //_ = stringBuilder.Append("ModuloInformacao em atualização: ").Append(emAtualizacao ? "Sim" : "Não").AppendLine();
            _ = stringBuilder.Append("Caminho do diretório: ").Append(DiretorioModulo).AppendLine();
            _ = stringBuilder.Append("Quantidade de DLLs no diretório: ").Append(Binarios.Count).AppendLine();
            _ = stringBuilder.Append("Quantidade de modulos inicializados: ").Append(ModulosInicializados).AppendLine();
            //_ = stringBuilder.Append("Tempo de atualização dos modulos: ").Append(_tempoAtualziacaoModulo).Append(" segundos").AppendLine();

            return stringBuilder.ToString();
        }


    }
}
