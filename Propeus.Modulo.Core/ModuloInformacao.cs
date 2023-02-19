using Propeus.Modulo.Abstrato.Util;
using Propeus.Modulo.Abstrato.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Propeus.Modulo.Abstrato.Atributos;

namespace Propeus.Modulo.Core
{
    /// <summary>
    /// Modelo para detalhar informações sobre o modulo
    /// </summary>
    public class ModuloInformacao : BaseModelo, IModuloInformacao
    {
        public ModuloInformacao(IModulo modulo)
        {
            if (modulo is null)
            {
                throw new ArgumentNullException(nameof(modulo));
            }

            Nome = modulo.Nome;
            Assembly = Assembly.GetExecutingAssembly();
            AssemblyName = Assembly.GetName();
            AssemblyLoadContext = AssemblyLoadContext.GetLoadContext(Assembly);
            Modulos = new Dictionary<string, IModuloTipo>
            {
                { Nome,new ModuloTipo(modulo) }
            };
            Version versao = AssemblyName.Version;
            NumeroVersaoAssembly = int.Parse(versao.Major.ToString(new CultureInfo("pt-BR")) + versao.Minor.ToString(new CultureInfo("pt-BR")) + versao.Build.ToString(new CultureInfo("pt-BR")), new CultureInfo("pt-BR"));
        }

        public ModuloInformacao(IModuloBinario moduloBinario)
        {
            if (moduloBinario is null)
            {
                throw new ArgumentNullException(nameof(moduloBinario));
            }

            Nome = moduloBinario.Caminho;
            Caminho = moduloBinario.Caminho;
            moduloBinario.Registrar(this);
            Modulos = new Dictionary<string, IModuloTipo>();
            MemoryStream = new WeakReference(moduloBinario.Memoria);


            AssemblyLoadContext = new ModuloAssemblyLoadContext();
            if (MemoryStream.IsAlive)
            {
                Assembly = AssemblyLoadContext.LoadFromStream(MemoryStream.Target as MemoryStream);
                (MemoryStream.Target as MemoryStream).Seek(0, SeekOrigin.Begin);
            }

            AssemblyName = Assembly.GetName();
            Version versao = AssemblyName.Version;
            foreach (string nomeModulo in ObterNomeModulos())
            {
                Modulos.Add(nomeModulo, new ModuloTipo(this, nomeModulo));
            }
            AssemblyLoadContext.Unloading += AssemblyLoadContext_Unloading;
            NumeroVersaoAssembly = int.Parse(versao.Major.ToString(new CultureInfo("pt-BR")) + versao.Minor.ToString(new CultureInfo("pt-BR")) + versao.Build.ToString(new CultureInfo("pt-BR")), new CultureInfo("pt-BR"));
            Hash = moduloBinario.Hash;
        }

        private void AssemblyLoadContext_Unloading(AssemblyLoadContext obj)
        {
            if (!(MemoryStream is null)  && MemoryStream.IsAlive)
            {
                ((MemoryStream)MemoryStream.Target).Dispose();
            }
        }


        private readonly WeakReference MemoryStream;
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
        /// Modulos mapeados do assembly
        /// </summary>
        public Dictionary<string, IModuloTipo> Modulos { get; }
        /// <summary>
        /// Caminho do modulo em disco
        /// </summary>
        public string Caminho { get; }
        /// <summary>
        /// Obtem o numero de versão do assembly mapeado
        /// </summary>
        public int NumeroVersaoAssembly { get; private set; }

        public string Hash { get; }

        protected override void Dispose(bool disposing)
        {
            foreach (KeyValuePair<string, IModuloTipo> modulo in Modulos)
            {
                if (!modulo.Value.Disposed)
                {
                    modulo.Value.Dispose();
                }
            }
            Assembly = null;
            AssemblyName = null;
            if (AssemblyLoadContext.IsCollectible)
            {
                AssemblyLoadContext.Unload();
            }
            AssemblyLoadContext = null;
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

        public Type ObterTipoModulo(string nomeModulo)
        {
            return Assembly.GetTypes().FirstOrDefault(x => x.Name == nomeModulo);
        }
    }
}
