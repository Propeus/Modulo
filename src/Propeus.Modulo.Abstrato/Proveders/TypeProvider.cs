using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato.Proveders
{
    internal class TypeInfo
    {
        private readonly Dictionary<string, WeakReference<Type>> contratos;

        public TypeInfo(Type type)
        {
            contratos = new Dictionary<string, WeakReference<Type>>();
            Referencia = new WeakReference<Type>(type);
            QuantidadeReferencia = 0;
            Criado = DateTime.Now;
            UltimaModificacao = default;

        }

        public WeakReference<Type> Referencia { get; private set; }
        public int QuantidadeReferencia { get; private set; }
        public DateTime Criado { get; }
        public DateTime? UltimaModificacao { get; private set; }

        public void AdicionarContrato(Type contrato)
        {
            if (!contratos.ContainsKey(contrato.FullName) && contrato.IsInterface && contrato.PossuiAtributo<ModuloContratoAttribute>())
            {
                contratos.Add(contrato.FullName, new WeakReference<Type>(contrato));
            }
        }

        public IEnumerable<Type> ObterContratos()
        {
            foreach (WeakReference<Type> item in contratos.Values)
            {
                if (item.TryGetTarget(out Type target))
                {
                    yield return target;
                }
            }
        }

        public void AtualizarReferencia(Type type)
        {
            Referencia.SetTarget(type);
            UltimaModificacao = DateTime.Now;
            QuantidadeReferencia = 0;
        }

        public Type GetReference()
        {
            if (Referencia.TryGetTarget(out Type target))
            {
                QuantidadeReferencia++;
                return target;
            }
            return null;
        }
    }

    /// <summary>
    /// Gerecia todas os tipos da aplicacao
    /// </summary>
    public static class TypeProvider
    {
        private static readonly ConcurrentDictionary<string, TypeInfo> Types = new();
        static TypeProvider()
        {
            string dir = Directory.GetCurrentDirectory();

            IEnumerable<Type> ls = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(x => x.Location.Contains(dir))//Filtrar para obter dll de um determinado diretorio
               .SelectMany(x => x.GetTypes())
               .Where(x => x.GetCustomAttribute<ModuloAttribute>() != null);

            foreach (Type item in ls)
            {
                _ = Types.TryAdd(item.Name, new TypeInfo(item));
            }
        }

        public static IEnumerable<Type> ObterAutoInicializavel()
        {
            foreach (KeyValuePair<string, TypeInfo> item in Types)
            {
                if (item.Value.Referencia.TryGetTarget(out Type target))
                {
                    if (target.PossuiAtributo<ModuloAutoInicializavelAttribute>())
                    {
                        yield return target;
                    }
                }
            }
        }

        public static IEnumerable<Type> ObterContratos(string name)
        {
            return Types.ContainsKey(name) ? Types[name].ObterContratos() : null;
        }

        public static IEnumerable<Type> ObterContratos(Type tipo)
        {
            return Types.ContainsKey(tipo.Name) ? Types[tipo.Name].ObterContratos() : null;
        }

        public static void AdicionarCntrato(string name, Type contrato)
        {
            if (Types.ContainsKey(name))
            {
                Types[name].AdicionarContrato(contrato);
            }
        }

        public static void AdicionarContrato(Type tipo, Type contrato)
        {
            if (Types.ContainsKey(tipo.Name))
            {
                Types[tipo.Name].AdicionarContrato(contrato);
            }
        }

        public static void AddOrUpdate(Type type)
        {

            _ = Types.AddOrUpdate(type.Name, new TypeInfo(type), updateValueFactory: (o, n) =>
            {
                if (Types.ContainsKey(o))
                {

                    if (Types[o].GetReference() == type)
                    {
                        return n;
                    }

                    Types[o].AtualizarReferencia(type);
                    return Types[o];
                }
                else
                {
                    return new TypeInfo(type);
                }
            });


        }

        public static Type Get(string name)
        {
            return Types.ContainsKey(name) ? Types[name].GetReference() : null;
        }

        public static int GetCount(string name)
        {
            return Types[name].QuantidadeReferencia;

        }

        public static void Remove(string name)
        {
            _ = Types.TryRemove(name, out _);
        }
    }
}