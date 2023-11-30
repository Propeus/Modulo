using System.Reflection.Emit;

using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Geradores
{

    internal static partial class Constantes
    {


        public const string CONSTNMEASSEMBLY = "IL_Gerador_Assembly_";
        public const string CONSTNMEMODULO = "IL_Gerador_Modulo_";

        public static string GerarNomeModulo()
        {
            return GerarNome(CONSTNMEMODULO);
        }

        public static string GerarNome(string @const)
        {
            return @const + Guid.NewGuid().ToString().Replace('-', '_');
        }
    }

    /// <summary>
    /// Classe para montagem inicial do Assembly
    /// </summary>
    public partial class ILGerador : IDisposable
    {
        /// <summary>
        /// Construtor de assembly
        /// </summary>
        internal AssemblyBuilder? _assemblyBuilder => _builderProxy.GetBuilder<AssemblyBuilder>();
        /// <summary>
        /// Classe de builders
        /// </summary>
        private ILBuilderProxy _builderProxy;

        /// <summary>
        /// Modulo do assembly
        /// </summary>
        public ILModulo Modulo { get; private set; }



        /// <summary>
        /// ObjectBuilder do gerador de IL
        /// </summary>
        /// <param name="nomeAssembly">ClassName do assembly</param>
        /// <param name="nomeModulo">Nome do modulo .NET</param>
        public ILGerador(string nomeAssembly = Constantes.CONSTNMEASSEMBLY, string nomeModulo = Constantes.CONSTNMEMODULO)
        {

            if (nomeAssembly == Constantes.CONSTNMEASSEMBLY)
            {
                nomeAssembly = Constantes.GerarNome(Constantes.CONSTNMEASSEMBLY);
            }

           var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(nomeAssembly), AssemblyBuilderAccess.RunAndCollect);

            if (nomeModulo == Constantes.CONSTNMEMODULO)
            {
                nomeModulo = Constantes.GerarNomeModulo();
            }

            _builderProxy = new ILBuilderProxy(new object[] { assemblyBuilder });

            Modulo = new ILModulo(nomeModulo, _builderProxy);
        }

        private bool disposedValue;

        /// <summary>
        /// Descarta o assembly gerado
        /// </summary>
        /// <param name="disposing">Indica se deve ser descartado o assembly</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Modulo.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Descarta o assembly gerado
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}