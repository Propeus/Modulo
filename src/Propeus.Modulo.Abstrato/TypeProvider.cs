using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Abstrato
{

    class TypeInfo
    {
        public TypeInfo(Type type)
        {

            Reference = new WeakReference<Type>(type);
            CountReference = 0;
            DateCreated = DateTime.Now;
            LastModified = default(DateTime?);

        }

        public WeakReference<Type> Reference { get; private set; }
        public int CountReference { get; private set; }
        public DateTime DateCreated { get; }
        public DateTime? LastModified { get; private set; }

        public void UpdateReference(Type type)
        {
            Reference.SetTarget(type);
            LastModified = DateTime.Now;
            CountReference = 0;
        }

        public Type GetReference()
        {
            Reference.TryGetTarget(out Type target);
            return target;
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

                    Types[o].UpdateReference(type);
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
            return Types[name].CountReference;

        }

    }
}