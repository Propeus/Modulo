using System;
using System.Reflection.Emit;

namespace Propeus.Modulo.IL.Geradores
{

    public static partial class Constantes
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

        private readonly string nomeAssembly;
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

            this.nomeAssembly = nomeAssembly;

            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(nomeAssembly), AssemblyBuilderAccess.RunAndCollect);
        }

        internal ILModulo Modulo { get; private set; }

        public ILModulo CriarModulo(string nomeModulo = Constantes.CONSTNMEMODULO)
        {
            if (Modulo == null)
            {
                if (nomeModulo == Constantes.CONSTNMEMODULO)
                {
                    nomeModulo = Constantes.GerarNomeModulo();
                }

                ILModulo modulo = new(this, nomeModulo);
                Modulo = modulo;
                return modulo;
            }
            else
            {
                throw new InvalidOperationException("Nao e permitido criar mais de um modulo por assembly");
            }

        }




        public override string ToString()
        {

            return Modulo?.ToString();
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Modulo.Dispose();
                    Modulo = null;
                    assemblyBuilder = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ILGerador()
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
}