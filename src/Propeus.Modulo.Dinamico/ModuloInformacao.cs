using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Dinamico
{
    /// <summary>
    /// Modelo para detalhar informações sobre o modulo
    /// </summary>
    public class ModuloInformacao : ModeloBase, IModuloInformacao
    {
        /// <summary>
        /// Inicializa o objeto obtendo as informacoes sobre o tipo informado
        /// </summary>
        /// <param name="moduloTipo">Tipo do modulo</param>
        /// <exception cref="ArgumentNullException">Argumento nulo</exception>
        public ModuloInformacao(Type moduloTipo)
        {
            if (moduloTipo is null)
            {
                throw new ArgumentNullException(nameof(moduloTipo));
            }


            Nome = moduloTipo.Name;
            Caminho = moduloTipo.Assembly.Location;

            Assembly = moduloTipo.Assembly;
            AssemblyName = moduloTipo.Assembly?.GetName();
            Version versao = AssemblyName.Version;
            Versao = $"{versao.Major}.{versao.Minor}.{versao.Build}";

            Modulos = new Dictionary<string, IModuloTipo>();
            Contratos = new Dictionary<string, List<Type>>();

            foreach (var item in ObterNomeModulos())
            {
                Modulos.Add(item, null);
            }
        }

        /// <summary>
        /// Inicializa o objeto obtendo as informacoes sobre o binario carregado
        /// </summary>
        /// <param name="moduloBinario"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ModuloInformacao(IModuloBinario moduloBinario)
        {
            if (moduloBinario is null)
            {
                throw new ArgumentNullException(nameof(moduloBinario));
            }



            Nome = moduloBinario.Caminho;
            Caminho = moduloBinario.Caminho;
            Modulos = new Dictionary<string, IModuloTipo>();
            Contratos = new Dictionary<string, List<Type>>();

            var assem = AppDomain.CurrentDomain.GetAssemblies().Select(asm => asm.Location).ToArray();
            //Nao pode carregaar em memoria um modulo que esta em execucao
            if (!assem.Contains(Caminho))
            {
                WeakReferenceAssembly = new WeakReference(moduloBinario.Memoria);

                AssemblyLoadContext = new ModuloAssemblyLoadContext();
                AssemblyLoadContext.Unloading += AssemblyLoadContext_Unloading;
                if (WeakReferenceAssembly.IsAlive)
                {
                    Assembly = AssemblyLoadContext.LoadFromStream(WeakReferenceAssembly.Target as MemoryStream);
                    _ = (WeakReferenceAssembly.Target as MemoryStream).Seek(0, SeekOrigin.Begin);
                }

                AssemblyName = Assembly.GetName();
            }
            else
            {
                Assembly = System.Reflection.Assembly.GetEntryAssembly();
                AssemblyName = Assembly.GetName();

            }
            Version versao = AssemblyName.Version;
            Versao = $"{versao.Major}.{versao.Minor}.{versao.Build}";

            foreach (var item in ObterNomeModulos())
            {
                Modulos.Add(item, null);
            }
        }
        ///<inheritdoc/>
        public override string Versao { get; }
        ///<inheritdoc/>
        public IModuloTipo this[string nome]
        {
            get
            {
                return Modulos[nome];
            }
            set
            {


                if (Modulos.ContainsKey(nome))
                {
                    if (value is null)
                    {
                        Modulos[nome]?.Dispose();
                    }
                    Modulos[nome] = value;
                }
                else
                {
                    Modulos.Add(nome, value);
                }

            }
        }

        private WeakReference WeakReferenceAssembly;
        /// <summary>
        /// Assembly a qual o modulo pertence
        /// </summary>
        public Assembly Assembly { get; private set; }
        /// <summary>
        /// Informações sobre o assembly do modulo
        /// </summary>
        public AssemblyName AssemblyName { get; private set; }
        /// <summary>
        /// Contexto do assembly
        /// </summary>
        private AssemblyLoadContext AssemblyLoadContext { get; set; }
        /// <summary>
        /// ModuloInformacao mapeados do assembly
        /// </summary>
        public Dictionary<string, IModuloTipo> Modulos { get; }
        Dictionary<string, List<Type>> Contratos { get; }
        /// <summary>
        /// Caminho do modulo em disco
        /// </summary>
        public string Caminho { get; }

        ///<inheritdoc/>
        public int ModulosDescobertos => Modulos?.Count ?? 0;
        ///<inheritdoc/>
        public int ModulosCarregados => Modulos?.Count(predicate: x => x.Value is not null) ?? 0;

        /// <summary>
        /// Adiciona um contrato atrelado a um modulo
        /// </summary>
        /// <param name="nomeModulo">Nome do modulo a ser vinculado o contrato</param>
        /// <param name="contrato">Tipo da interface de contrato</param>
        public void AdicionarContrato(string nomeModulo, Type contrato)
        {
            if (string.IsNullOrEmpty(nomeModulo))
            {
                throw new ArgumentException($"'{nameof(nomeModulo)}' não pode ser nulo nem vazio.", nameof(nomeModulo));
            }

            if (contrato is null)
            {
                throw new ArgumentNullException(nameof(contrato));
            }

            if (!contrato.IsInterface)
                throw new ArgumentException("O tipo deve ser uma interface", nameof(contrato));

            if (Contratos.ContainsKey(nomeModulo))
            {
                if (!Contratos[nomeModulo].Contains(contrato))
                {
                    Contratos[nomeModulo].Add(contrato);
                }
            }
            else
            {
                Contratos.Add(nomeModulo, new List<Type> { contrato });
            }
        }
        /// <summary>
        /// Obtem a lista de contratos do modulo informado
        /// </summary>
        /// <param name="nomeModulo"></param>
        /// <returns></returns>
        public List<Type> ObterContratos(string nomeModulo)
        {
            if (Contratos.ContainsKey(nomeModulo))
            {
                return Contratos[nomeModulo];
            }
            return null;
        }
        ///<inheritdoc/>
        public bool PossuiModulo(string nomeModulo) => Modulos.ContainsKey(nomeModulo);
        ///<inheritdoc/>
        public Type CarregarTipoModulo(string nomeModulo)
        {
            if (Modulos.ContainsKey(nomeModulo))
            {
                return Assembly.GetTypes().Single(x => x.Herdado<IModulo>() && x.PossuiAtributo<ModuloAttribute>() && x.Name == nomeModulo);
            }
            return null;
        }
        ///<inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());

            _ = sb.Append("Versao do assembly: ").Append(Versao).AppendLine();
            _ = sb.Append("Caminho completo do modulo: ").Append(Caminho).AppendLine();
            _ = sb.Append("Quantidade de modulos descobertos: ").Append(ModulosDescobertos).AppendLine();
            _ = sb.Append("Quantidade de modulos carregados: ").Append(ModulosCarregados).AppendLine();

            return sb.ToString();
        }
        ///<inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            Modulos.Clear();
            Contratos.Clear();

            Assembly = null;
            AssemblyName = null;

            if (AssemblyLoadContext is not null)
            {
                if (AssemblyLoadContext.IsCollectible)
                {
                    AssemblyLoadContext.Unload();
                }
                AssemblyLoadContext = null;
            }

            base.Dispose(disposing);
        }



        /// <summary>
        /// Metodo estatico para validação de modulo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="regra"></param>
        /// <returns></returns>
        public static bool PossuiModuloValido(IModuloBinario path, params IRegra[] regra)
        {
            bool result = true;
            foreach (IRegra item in regra)
            {
                result = item.Executar(path);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }
        /// <summary>
        /// Obtem os nomes dos modulos mapeados
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> ObterNomeModulos()
        {

            IEnumerable<Type> src = Assembly.GetTypes().Where(x => x.Herdado<IModulo>() && x.PossuiAtributo<ModuloAttribute>());
            foreach (Type nme in src)
            {
                yield return nme.Name;
            }
        }

        private void AssemblyLoadContext_Unloading(AssemblyLoadContext obj)
        {
            if (WeakReferenceAssembly is not null && WeakReferenceAssembly.IsAlive)
            {
                ((MemoryStream)WeakReferenceAssembly.Target).Dispose();
                WeakReferenceAssembly = null;
            }
        }
    }
}
