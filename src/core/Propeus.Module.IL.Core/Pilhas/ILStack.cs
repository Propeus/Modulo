using System.Reflection.Emit;

using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

namespace Propeus.Module.IL.Core.Pilhas
{
    /// <summary>
    /// Pilha base para a construção das demais pilhas de instrução IL
    /// </summary>
    internal class ILStack : IILPilha
    {
        /// <summary>
        /// Construtor para criar instrução IL 
        /// </summary>
        /// <param name="scopeBuilder">Escopo onde será aplicado a instrução IL</param>
        /// <param name="opCode">Instrução IL a ser aplicado no escopo</param>
        /// <exception cref="ArgumentNullException">Caso o escopo seja nulo</exception>
        public ILStack(ILBuilderProxy scopeBuilder, OpCode opCode)
        {
            ScopeBuilder = scopeBuilder ?? throw new ArgumentNullException(nameof(scopeBuilder));
            Code = opCode;

        }

        /// <summary>
        /// Construtor para criar instrução IL 
        /// </summary>
        /// <remarks>
        /// Este construtor normalmente é usado para instruções que dependem de índice, exemplo o <see cref="OpCodes.Ldarg"/>
        /// </remarks>
        /// <param name="scopeBuilder">Escopo onde será aplicado a instrução IL</param>
        /// <param name="opCode">Instrução IL a ser aplicado no escopo</param>
        /// <param name="index"></param>
        public ILStack(ILBuilderProxy scopeBuilder, OpCode opCode, int index) : this(scopeBuilder, opCode)
        {

            if (opCode == OpCodes.Ldarg)
            {
                Code = index switch
                {
                    0 => OpCodes.Ldarg_0,
                    1 => OpCodes.Ldarg_1,
                    2 => OpCodes.Ldarg_2,
                    3 => OpCodes.Ldarg_3,
                    _ => OpCodes.Ldarg_S,
                };
            }
        }
        /// <summary>
        /// Instancia do builder que será aplicado o IL
        /// </summary>
        public ILBuilderProxy ScopeBuilder { get; private set; }
        /// <summary>
        /// Instrução IL
        /// </summary>
        public OpCode Code { get; }

        /// <summary>
        /// Indicador de execução da instrução
        /// </summary>
        protected bool _executado;
        /// <summary>
        /// Indicador de 'linhas' de instrução do IL
        /// </summary>
        protected int _offset;

        /// <summary>
        /// Executa a instrução IL quando chamado
        /// </summary>
        /// <exception cref="ObjectDisposedException">Será acionado quando o objeto atual for chamado pelo <see cref="Dispose()"/></exception>
        public virtual void Apply()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            _executado = true;
            _offset = ScopeBuilder?.ILGenerator?.ILOffset ?? 0;
        }
        /// <summary>
        /// Indica que o objeto foi coletado pelo G.C.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Coleta os objetos desta pilha
        /// </summary>
        /// <param name="disposing">Indica se deve coletar os objetos gerenciados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && ScopeBuilder is not null)
                {
                    ScopeBuilder.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Coleta os objetos desta pilha
        /// </summary>
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
