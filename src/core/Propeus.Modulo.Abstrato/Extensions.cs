using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract
{
    public static class Extensions
    {
        /// <summary>
        /// Aguarda um modulo entrar no estado desejado
        /// </summary>
        /// <param name="module">Modulo a ser aguardado</param>
        /// <param name="state">Estado esperado</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void WaitModuleState(this IModule module, State state, CancellationToken? cancellationToken = null)
        {
            if (module is null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (cancellationToken != null)
            {
                do
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                } while (module.State != state || !cancellationToken.Value.IsCancellationRequested);
            }
            else
            {
                do
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                } while (module.State != state);
            }
        }
    }
}
