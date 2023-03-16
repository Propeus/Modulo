﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato.Proveders
{
    internal class Modulo : IDisposable
    {
        public Modulo(string filePath)
        {
            Valido = false;
            Local = filePath;
            Modulos = new System.Collections.Generic.HashSet<string>();
            CarregarDll();
        }

        public Type this[string name] => Modulos.Contains(name) ? TypeProvider.Get(name) : null;

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


            string nHash = null;
            using (MemoryStream reader = new())
            {
                using (FileStream arquivo = new(Local, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader binario = new(arquivo))
                    {
                        binario.BaseStream.CopyTo(reader);
                        binario.Close();
                    }
                    arquivo.Close();
                }
                _ = reader.Seek(0, SeekOrigin.Begin);
                nHash = reader.ToArray().Hash();
            }

            if (Hash != nHash)
            {
                if (AssemblyLoadContext != null)
                {
                    AssemblyLoadContext.Unload();
                    CurrentAssembly.SetTarget(ObterModulo(Local));
                }
                else
                {
                    AssemblyLoadContext = new AssemblyLoadContext(null, true);
                    AssemblyLoadContext.Unloading += AssemblyLoadContext_Unloading;
                    CurrentAssembly = new WeakReference<Assembly>(ObterModulo(Local));
                }
            }

            if (CurrentAssembly.TryGetTarget(out Assembly target))
            {
                Modulos.Clear();
                IEnumerable<Type> lsAux = target.GetTypes().Where(x => x.Herdado<IModulo>() && x.PossuiAtributo<ModuloAttribute>());
                foreach (Type item in lsAux)
                {
                    Modulos.Add(item.Name);
                    TypeProvider.AddOrUpdate(item);
                }
                Valido = Modulos.Any();
            }

        }

        private void AssemblyLoadContext_Unloading(AssemblyLoadContext obj)
        {
            //TODO: Adicionar evento?
        }
        private Assembly ObterModulo(string location)
        {
            using MemoryStream ms = new();
            using (FileStream arquivo = new(location, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binario = new(arquivo))
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
        public AssemblyLoadContext AssemblyLoadContext { get; private set; }
        public WeakReference<Assembly> CurrentAssembly { get; private set; }
        public ICollection<string> Modulos { get; private set; }

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

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Modulo()
        // {
        //     // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public static class ModuleProvider
    {

        static ModuleProvider()
        {
            Modulos = new ConcurrentDictionary<string, Modulo>();
            ModulosIgnorados = new ConcurrentDictionary<string, Modulo>();
            TipoModulo = new ConcurrentDictionary<string, Modulo>();

            DiretorioAtual = Directory.GetCurrentDirectory();
        }

        public static string DiretorioAtual { get; private set; }
        public static int ModulosCarregados => TipoModulo.Count;
        public static int ModulosDllCarregados => Modulos.Count;

        private static readonly ConcurrentDictionary<string, Modulo> Modulos;
        private static readonly ConcurrentDictionary<string, Modulo> ModulosIgnorados;
        private static readonly ConcurrentDictionary<string, Modulo> TipoModulo;

        public static Type ObterTipoModuloAtual(string name)
        {
            return TypeProvider.Get(name);
        }

        public static void RecarregamentoRapido()
        {
            CarregarModulos(Modulos.Keys.ToArray());
        }
        public static void Recarregamento()
        {
            CarregarModulos(Modulos.Keys.ToArray());
            CarregarModulos(ModulosIgnorados.Keys.ToArray());
        }
        public static void RecarregmentoCompleto()
        {
            string[] dlls = Directory.GetFiles(DiretorioAtual, "*.dll");
            CarregarModulos(dlls);
        }

        private static void CarregarModulos(string[] dlls)
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

    }
}
