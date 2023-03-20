using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Security.Cryptography;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;
using Propeus.Modulo.Util.Atributos;
using Propeus.Modulo.Util.Objetos;
using Propeus.Modulo.Util.Tipos;

namespace Propeus.Modulo.Abstrato.Proveders
{
    class Modulo : IDisposable
    {
        public Modulo(string filePath)
        {
            Valido = false;
            Local = filePath;
            Modulos = new System.Collections.Generic.HashSet<string>();
            CarregarDll();
        }

        public Type this[string name] => Modulos.Contains(name) ? TypeProvider.Provider.Get(name) : null;

        public void Recarregar()
        {
            CarregarDll();
        }

        private void CarregarDll()
        {
            DateTime lwt = new FileInfo(Local).LastWriteTime;
            if (UltimaModificacao == null || UltimaModificacao != lwt)
            {
                UltimaModificacao = new FileInfo(Local).LastWriteTime;
            }
            else
            {
                return;
            }

            var auxTamanho = FileLength(Local);
            var auxHash = HashFile(Local);

            if (Tamanho == 0
                || Tamanho != auxTamanho
                || Hash is null
                || Hash != auxHash)
            {
                Tamanho = auxTamanho;
                Hash = auxHash;

                if (AssemblyLoadContext != null)
                {
                    this.NotificarAviso($"Descarregamento de modulo acionado. {Hash}=>{auxHash}");
                    AssemblyLoadContext.Unload();
                    AssemblyLoadContext = new AssemblyLoadContext(null, true);
                    CurrentAssembly.SetTarget(ObterModulo(Local));
                }
                else
                {
                    AssemblyLoadContext = new AssemblyLoadContext(null, true);
                    AssemblyLoadContext.Unloading += AssemblyLoadContext_Unloading;
                    CurrentAssembly = new WeakReference<Assembly>(ObterModulo(Local));
                }

                if (CurrentAssembly.TryGetTarget(out Assembly target))
                {
                    Modulos.Clear();
                    IEnumerable<Type> lsAux = target.GetTypes().Where(x => x.Herdado<IModulo>() && x.PossuiAtributo<ModuloAttribute>());
                    foreach (Type item in lsAux)
                    {
                        Modulos.Add(item.Name);
                        TypeProvider.Provider.AddOrUpdate(item);
                    }
                    Valido = Modulos.Any();
                }
            }




        }

        private long FileLength(string filename)
        {
            var fi = new FileInfo(filename);
            return fi.Length;

        }

        private string HashFile(string filename)
        {

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var fsh = MD5.Create().ComputeHash(fs);
                return BitConverter.ToString(fsh);
            }
        }

        private void AssemblyLoadContext_Unloading(AssemblyLoadContext obj)
        {
            this.NotificarAviso(obj.Name + " foi descarregado da aplicacao");
        }
        private Assembly ObterModulo(string location)
        {
            using MemoryStream ms = new();
            using (FileStream arquivo = new FileStream(location, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binario = new BinaryReader(arquivo))
                {
                    binario.BaseStream.CopyTo(ms);
                    binario.Close();
                }
                arquivo.Close();
            }
            _ = ms.Seek(0, SeekOrigin.Begin);
            return AssemblyLoadContext.LoadFromStream(ms);
        }

        public bool Valido { get; private set; }
        public DateTime? UltimaModificacao { get; private set; }
        public string Local { get; private set; }
        public string Hash { get; private set; }
        public long Tamanho { get; private set; }
        public AssemblyLoadContext AssemblyLoadContext { get; private set; }
        public WeakReference<Assembly> CurrentAssembly { get; private set; }
        public ICollection<string> Modulos { get; private set; }

        private MemoryStream MemoryStreamms;

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AssemblyLoadContext.Unload();
                    Modulos.Clear();
                }
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'DisposeMethod(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Provedor de modulos (DLL)
    /// </summary>
    public class ModuleProvider : IDisposable
    {
        static ModuleProvider _provider;
        /// <summary>
        /// Obtem um provedor de modulos existente ou cria um novo
        /// </summary>
        public static ModuleProvider Provider
        {
            get
            {
                if (_provider is null || _provider.disposedValue)
                {
                    _provider = new ModuleProvider();
                }

                return _provider;
            }

        }

        /// <summary>
        /// Indica o diretorio que esta sendo observado
        /// </summary>
        /// <value>Diretorio do programa em execucao</value>
        public string DiretorioAtual { get; private set; } = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
        /// <summary>
        /// Quantidade de modulos mapeados
        /// </summary>
        public int ModulosCarregados => TipoModulo?.Count ?? 0;
        /// <summary>
        /// Quantidade de DLLs carregados
        /// </summary>
        public int ModulosDllCarregados => Modulos?.Count ?? 0;

        private ConcurrentDictionary<string, Modulo> Modulos = new ConcurrentDictionary<string, Modulo>();
        private ConcurrentDictionary<string, Modulo> ModulosIgnorados = new ConcurrentDictionary<string, Modulo>();
        private ConcurrentDictionary<string, Modulo> TipoModulo = new ConcurrentDictionary<string, Modulo>();

        /// <summary>
        /// Obtem o tipo do modulo atual
        /// </summary>
        /// <param name="name">Nome do tipo</param>
        /// <returns></returns>
        public Type ObterTipoModuloAtual(string name)
        {
            return TypeProvider.Provider.Get(name);
        }

        /// <summary>
        /// Recarrega todos os tipos validos
        /// </summary>
        public void RecarregamentoRapido()
        {
            CarregarModulos(Modulos.Keys.ToArray());
        }
        /// <summary>
        /// Recarrega todos os tipos validos e invalidos, garantindo que o tipo invalido foi corrigido e vice e versa
        /// </summary>
        public void Recarregamento()
        {
            CarregarModulos(Modulos.Keys.ToArray());
            CarregarModulos(ModulosIgnorados.Keys.ToArray());
        }

        /// <summary>
        /// Carrega todas as DLLs incluindo as validas e invalidas
        /// </summary>
        public void RecarregmentoCompleto()
        {
            string[] dlls = Directory.GetFiles(DiretorioAtual, "*.dll");
            CarregarModulos(dlls);
        }

        private void CarregarModulos(string[] dlls)
        {

            //Possivel tratamento de descarregamento de assembly para evitar vazamento de memoria
            foreach (string dll in dlls)
            {
                if (ModulosIgnorados.ContainsKey(dll) || Modulos.ContainsKey(dll))
                {
                    if (Modulos.TryGetValue(dll, out Modulo target))
                    {
                        target.Recarregar();
                        if (!target.Valido)
                        {
                            _ = Modulos.TryRemove(dll, out _);
                            _ = ModulosIgnorados.TryAdd(dll, target);

                            IEnumerable<string> keys = TipoModulo.Where(x => x.Value == target).Select(x => x.Key);
                            foreach (string item in keys)
                            {
                                _ = TipoModulo.TryRemove(item, out _);
                            }
                        }


                    }
                    else if (ModulosIgnorados.TryGetValue(dll, out Modulo targetIgnored))
                    {
                        targetIgnored.Recarregar();
                        if (targetIgnored.Valido)
                        {
                            _ = ModulosIgnorados.TryRemove(dll, out _);
                            _ = Modulos.TryAdd(dll, targetIgnored);
                            foreach (string item in targetIgnored.Modulos)
                            {
                                _ = TipoModulo.TryAdd(item, targetIgnored);
                            }
                        }
                    }
                    continue;
                }

                Modulo modulo = new(dll);
                if (modulo.Valido)
                {
                    if (Modulos.TryAdd(dll, modulo))
                    {
                        foreach (string moduleName in modulo.Modulos)
                        {
                            _ = TipoModulo.TryAdd(moduleName, modulo);
                        }
                    }
                }
                else
                {
                    _ = ModulosIgnorados.TryAdd(dll, modulo);
                }
            }
        }

        private bool disposedValue;

        /// <summary>
        /// Remove todos os modulos carregados em memoria
        /// </summary>
        /// <param name="disposing">Indica se deve ser liberado os objetos gerenciaveis da memoria</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in Modulos.Values)
                    {
                        item.Dispose();
                    }

                    foreach (var item in ModulosIgnorados.Values)
                    {
                        item.Dispose();
                    }

                    foreach (var item in TipoModulo.Values)
                    {
                        item.Dispose();
                    }


                    Modulos.Clear();
                    ModulosIgnorados.Clear();
                    TipoModulo.Clear();

                }

                Modulos = null;
                ModulosIgnorados = null;
                TipoModulo = null;

                disposedValue = true;
            }
        }


        /// <summary>
        /// Remove todos os modulos carregados em memoria
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
