using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Propeus.Modulo.IL.Geradores
{
    public class ILDelegate : ILClasse
    {
        public ILDelegate(Proxy.ILBuilderProxy IlProxy,
                          string nome,
                          string @namespace,
                          Enums.Token[] acessadores) : base(IlProxy,
                                                                                               nome,
                                                                                               @namespace,
                                                                                               typeof(System.MulticastDelegate),
                                                                                               null,
                                                                                               acessadores)
        {

        }

        public ConstructorInfo ConstructorInfo => Proxy.ObterBuilder<TypeBuilder>()?.GetConstructor(new Type[] { typeof(object), typeof(nint) });
        public MethodInfo InvokeInfo => Proxy.ObterBuilder<TypeBuilder>()?.GetMethod("Invoke");
    }
}
