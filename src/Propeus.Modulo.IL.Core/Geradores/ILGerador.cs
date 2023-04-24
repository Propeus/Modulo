using System;
using System.Reflection.Emit;

namespace Propeus.Modulo.IL.Geradores
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

        internal AssemblyBuilder assemblyBuilder;



        /// <summary>
        /// Construtor do gerador de IL
        /// </summary>
        /// <param name="nomeAssembly">Nome do assembly</param>
        public ILGerador(string nomeAssembly = Constantes.CONSTNMEASSEMBLY)
        {



            if (nomeAssembly == Constantes.CONSTNMEASSEMBLY)
            {
                nomeAssembly = Constantes.GerarNome(Constantes.CONSTNMEASSEMBLY);
            }

            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(nomeAssembly), AssemblyBuilderAccess.RunAndCollect);
        }

        internal ILModulo Modulo { get; private set; }

        internal ILModulo CriarModulo(string nomeModulo = Constantes.CONSTNMEMODULO)
        {
            if (nomeModulo == Constantes.CONSTNMEMODULO)
            {
                nomeModulo = Constantes.GerarNomeModulo();
            }

            ILModulo modulo = new ILModulo( this, nomeModulo);
            Modulo = modulo;
            return modulo;
        }




        internal bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Modulo?.Dispose();
                    Modulo = null;
                    assemblyBuilder = null;
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
}