using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Classe de extensão para modulos
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Aguarda um modulo entrar no estado desejado
        /// </summary>
        /// <param name="module">Modulo a ser aguardado</param>
        /// <param name="state">Estado esperado</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <exception cref="ArgumentNullException">Quando modulo for nulo</exception>
        public static void WaitModuleState(this IModule module, State state, CancellationToken? cancellationToken = null)
        {
            ArgumentNullException.ThrowIfNull(module);

            if (cancellationToken is null)
            {
                var cts = new CancellationTokenSource();
                cancellationToken = cts.Token;
#if DEBUG
                cts.CancelAfter(TimeSpan.FromMilliseconds(10));
#elif RELEASE
                cts.CancelAfter(TimeSpan.FromSeconds(10));
#endif
            }

            while (module.State != state && !cancellationToken.Value.IsCancellationRequested)
            {
                Task.Delay(TimeSpan.FromMilliseconds(1)).Wait();
            }


        }
    }
}
