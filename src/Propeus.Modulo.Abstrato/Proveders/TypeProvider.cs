using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato.Proveders
{

    class TypeInfo
    {
        Dictionary<string,WeakReference<Type>> contratos;

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
                contratos.Add(contrato.FullName,new WeakReference<Type>(contrato));
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
        static ConcurrentDictionary<string, TypeInfo> Types = new ConcurrentDictionary<string, TypeInfo>();
        static TypeProvider()
        {
            var dir = Directory.GetCurrentDirectory();

            var ls = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(x => x.Location.Contains(dir))//Filtrar para obter dll de um determinado diretorio
               .SelectMany(x => x.GetTypes())
               .Where(x => x.GetCustomAttribute<ModuloAttribute>() != null);

            foreach (var item in ls)
            {
                Types.TryAdd(item.Name, new TypeInfo(item));
            }
        }

        public static IEnumerable<Type> ObterAutoInicializavel()
        {
            foreach (var item in Types)
            {
                if (item.Value.Referencia.TryGetTarget(out Type target))
                {
                    yield return target;
                }
            }
        }

        public static IEnumerable<Type> ObterContratos(string name)
        {
            if (Types.ContainsKey(name))
            {
                return Types[name].ObterContratos();
            }
            return null;
        }

        public static IEnumerable<Type> ObterContratos(Type tipo)
        {
            if (Types.ContainsKey(tipo.Name))
            {
                return Types[tipo.Name].ObterContratos();
            }
            return null;
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

            Types.AddOrUpdate(type.Name, new TypeInfo(type), updateValueFactory: (o, n) =>
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
            if (Types.ContainsKey(name))
            {
                return Types[name].GetReference();
            }

            return null;
        }

        public static int GetCount(string name)
        {
            return Types[name].QuantidadeReferencia;

        }

        public static void Remove(string name)
        {
            Types.TryRemove(name, out _);
        }
    }
}