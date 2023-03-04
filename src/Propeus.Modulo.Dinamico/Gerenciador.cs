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
using System.Reflection;
using Propeus.Modulo.Abstrato.Helpers;

namespace Propeus.Modulo.Dinamico
{
    [Modulo]
    public class Gerenciador : ModuloBase, IGerenciador, IGerenciadorArgumentos, IGerenciadorDiagnostico, IGerenciadorRegistro
    {
        public Gerenciador(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
            Binarios = new ConcurrentDictionary<string, IModuloBinario>();
            ModuloProvider = new Dictionary<string, ILClasseProvider>();
            DataInicio = DateTime.Now;
            _scheduler = new TaskJob(10);


            CARREGAR_MODULO_JOB(null);

            _scheduler.AddTask(CARREGAR_MODULO_JOB, TimeSpan.FromSeconds(1), "CARREGAR_MODULO_JOB");
            _scheduler.AddTask(AUTO_INICIALIZAR_MODULO_JOB, TimeSpan.FromSeconds(1), "AUTO_INICIALIZAR_MODULO_JOB");

        }


        public DateTime DataInicio { get; }

        private TaskJob _scheduler;

        public DateTime UltimaAtualizacao { get; private set; } = DateTime.Now;
        public int ModulosInicializados => ((Gerenciador is IGerenciadorDiagnostico) ? (Gerenciador as IGerenciadorDiagnostico).ModulosInicializados : -1);

        private ConcurrentDictionary<string, IModuloBinario> Binarios { get; }
        private Dictionary<string, ILClasseProvider> ModuloProvider { get; }


        public event Evento OnEvento;

        public string DiretorioModulo { get; set; } = Directory.GetCurrentDirectory();

        ///<inheritdoc/>
        public T Criar<T>() where T : IModulo
        {
            return (T)Criar(typeof(T));
        }
        ///<inheritdoc/>
        public IModulo Criar(string nomeModulo)
        {
            var binM = Binarios.Single(x => x.Value.ModuloInformacao.PossuiModulo(nomeModulo)).Value;

            return Criar(binM.ModuloInformacao.CarregarTipoModulo(nomeModulo));
        }
        ///<inheritdoc/>
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

            var ctors = modulo.GetConstructors();
            foreach (var ctor in ctors)
            {
                var @params = ctor.GetParameters();
                foreach (var @param in @params)
                {
                    if (param.ParameterType.IsInterface && param.ParameterType.PossuiAtributo<ModuloContratoAttribute>())
                    {
                        _ = ResoverContratos(param.ParameterType);
                    }
                }
            }

            return Gerenciador.Criar(modulo);


        }

        private Type ResoverContratos(Type contrato)
        {
            if (!contrato.IsInterface)
                throw new ArgumentException("O tipo nao e uma interface");

            if (contrato.PossuiAtributo<ModuloContratoAttribute>())
            {
                Type tipoImplementacao;
                IModuloInformacao moduloInfo;
                ILClasseProvider provider;

                var attr = contrato.ObterAtributo<ModuloContratoAttribute>();
                if (attr.Tipo is not null)
                {
                    moduloInfo = new ModuloInformacao(attr.Tipo);
                }
                else
                {
                    moduloInfo = Binarios.Single(bin => bin.Value.ModuloInformacao.PossuiModulo(attr.Nome)).Value.ModuloInformacao;

                }
                tipoImplementacao = moduloInfo.CarregarTipoModulo(attr.Nome);


                moduloInfo.AdicionarContrato(tipoImplementacao.Name, contrato);
                if (!ModuloProvider.ContainsKey(tipoImplementacao.Name))
                {
                    provider = GeradorHelper.ObterModuloGenerico().CriarProxyClasse(tipoImplementacao, moduloInfo.ObterContratos(tipoImplementacao.Name).ToArray());
                    ModuloProvider.Add(tipoImplementacao.Name, provider);
                }
                else
                {
                    ModuloProvider[tipoImplementacao.Name].NovaVersao(interfaces: moduloInfo.ObterContratos(tipoImplementacao.Name).ToArray());
                }

                ModuloProvider[tipoImplementacao.Name].Executar();
                contrato = ModuloProvider[tipoImplementacao.Name].ObterTipoGerado();

                if (attr.Tipo is not null)
                {
                    moduloInfo.Dispose();
                }

                return contrato;


            }
            else
            {
                //Interface nao mapeada
                throw new InvalidCastException();
            }
        }

        ///<inheritdoc/>
        public T Criar<T>(object[] args) where T : IModulo
        {


            T modulo = (T)Criar(typeof(T));

            var mthInstancia = modulo.GetType().GetMethod(Compartilhado.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            mthInstancia?.Invoke(modulo, args);

            return modulo;

        }
        ///<inheritdoc/>
        public IModulo Criar(Type modulo, object[] args)
        {
            IModulo iModulo = Gerenciador.Criar(modulo);

            var mthInstancia = iModulo.GetType().GetMethod(Compartilhado.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            mthInstancia?.Invoke(iModulo, args);

            return iModulo;


        }
        ///<inheritdoc/>
        public IModulo Criar(string nomeModulo, object[] args)
        {
            IModulo iModulo = Gerenciador.Criar(Nome);

            var mthInstancia = iModulo.GetType().GetMethod(Compartilhado.Constantes.METODO_INSTANCIA, args.Select(x => x.GetType()).ToArray());
            mthInstancia?.Invoke(iModulo, args);

            return iModulo;
        }

        ///<inheritdoc/>
        public void Remover<T>(T modulo) where T : IModulo
        {
            Gerenciador.Remover(modulo);
        }
        ///<inheritdoc/>
        public void Remover(string id)
        {
            if (Gerenciador.Existe(id))
            {
                Gerenciador.Remover(id);
            }
        }
        ///<inheritdoc/>
        public void RemoverTodos()
        {
            Gerenciador.RemoverTodos();
        }


        ///<inheritdoc/>
        public T Reiniciar<T>(T modulo) where T : IModulo
        {
            T nModulo = Gerenciador.Reiniciar(modulo);
            return nModulo;
        }
        ///<inheritdoc/>
        public IModulo Reiniciar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ERRO_ID_NULO_OU_VAZIO, nameof(id)), nameof(id));
            }

            return Gerenciador.Reiniciar(id);


        }


        ///<inheritdoc/>
        public T Obter<T>() where T : IModulo
        {
            Type type = typeof(T);
            return Obter(type).To<T>();
        }
        ///<inheritdoc/>
        public IModulo Obter(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            return Gerenciador.Obter(modulo);
        }
        ///<inheritdoc/>
        public IModulo Obter(string id)
        {
            IModulo modulo = Gerenciador.Obter(id);
            return modulo;
        }

        ///<inheritdoc/>
        public bool Existe(Type modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            return Gerenciador.Existe(modulo);
        }
        ///<inheritdoc/>
        public bool Existe(IModulo modulo)
        {
            return Gerenciador.Existe(modulo);
        }
        ///<inheritdoc/>
        public bool Existe(string id)
        {
            return Gerenciador.Existe(id);
        }

        ///<inheritdoc/>
        public IEnumerable<IModulo> Listar()
        {
            return Gerenciador.Listar();
        }

        ///<inheritdoc/>
        public async Task ManterVivoAsync()
        {
            await Gerenciador.ManterVivoAsync().ConfigureAwait(true);
        }

        public void Registrar(IModulo modulo)
        {
            (Gerenciador as IGerenciadorRegistro).Registrar(modulo);
        }

        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            RemoverTodos();
            base.Dispose(disposing);
        }

        ///<inheritdoc/>
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

        private void AUTO_INICIALIZAR_MODULO_JOB(object cancelationToken)
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
        }
        private void CARREGAR_MODULO_JOB(object cancelationToken)
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
        }


    }
}
