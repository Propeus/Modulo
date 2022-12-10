﻿using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Proxy;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Propeus.Modulo.IL.Geradores
{

    public static partial class Constantes
    {
 

        public const string CONST_NME_ASSEMBLY = "IL_Gerador_Assembly_";
        public const string CONST_NME_MODULO = "IL_Gerador_Modulo_";

        public static string GerarNomeModulo()
        {
            return GerarNome(CONST_NME_MODULO);
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
        /// <param name="nomeModulo">Nome do modulo</param>
        public ILGerador(string nomeAssembly = Constantes.CONST_NME_ASSEMBLY)
        {

            if (nomeAssembly == Constantes.CONST_NME_ASSEMBLY)
                nomeAssembly = Constantes.GerarNome(Constantes.CONST_NME_ASSEMBLY);

            this.nomeAssembly = nomeAssembly;
            
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(nomeAssembly), AssemblyBuilderAccess.RunAndCollect);
        }

        internal ILModulo Modulo { get; private set; }

        public ILModulo CriarModulo(string nomeModulo = Constantes.CONST_NME_MODULO)
        {
            if (Modulo == null)
            {
                if(nomeModulo == Constantes.CONST_NME_MODULO)
                {
                    nomeModulo = Constantes.GerarNomeModulo();
                }

                var modulo = new ILModulo(this, nomeModulo);
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