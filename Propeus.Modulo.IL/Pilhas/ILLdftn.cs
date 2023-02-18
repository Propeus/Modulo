using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.IL.Proxy;

namespace Propeus.Modulo.IL.Pilhas
{
    /// <summary>
    /// Cria um ponteiro para um metodo
    /// </summary>
    internal class ILLdftn : ILPilha
    {
        public ILLdftn(ILBuilderProxy iLBuilderProxy, MethodInfo methodInfo) : base(iLBuilderProxy, OpCodes.Ldftn)
        {
            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; }

        public override void Executar()
        {
            if (_executado)
            {
                return;
            }

            base.Executar();

            Proxy.Emit(Code, MethodInfo);
        }

        public override string ToString()
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Code).Append(' ').Append(MethodInfo.ReturnType.Name).Append(' ');
            stringBuilder.Append(MethodInfo.DeclaringType.FullName).Append("::").Append(MethodInfo.Name);
            stringBuilder.Append("(");
            stringBuilder.Append(string.Join(',', MethodInfo.GetParameters().Select(x => x.ParameterType.Name)));
            stringBuilder.Append(")");



            return stringBuilder.ToString();
        }
    }
}
