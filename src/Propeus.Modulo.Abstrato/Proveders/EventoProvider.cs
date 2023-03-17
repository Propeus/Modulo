using System;

namespace Propeus.Modulo.Abstrato.Proveders
{
    /// <summary>
    /// Classe para disparar mensagens para os ouvintes
    /// </summary>
    public static class EventoProvider
    {
        /// <summary>
        /// Delegate padrao
        /// </summary>
        /// <param name="fonte">Tipo do objeto que esta distribuindo a mensagem</param>
        /// <param name="mensagem">Conteudo</param>
        /// <param name="exception">Excecao se houver</param>
        public delegate void Evento(Type fonte, string mensagem, Exception exception);

        private static event Evento OnInfo;
        private static event Evento OnAviso;
        private static event Evento OnErro;

        /// <summary>
        /// Envia uma informacao para os ouvintes
        /// </summary>
        /// <param name="fonte">Tipo do objeto que esta distribuindo a mensagem</param>
        /// <param name="mensagem">Conteudo</param>
        public static void NotificarInformacao(this object fonte, string mensagem)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnInfo?.Invoke(type, mensagem, null);
            }
            else
            {
                OnInfo?.Invoke(fonte as Type, mensagem, null);
            }
        }
        /// <summary>
        /// Envia uma informacao para os ouvintes
        /// </summary>
        /// <param name="fonte">Tipo do objeto que esta distribuindo a mensagem</param>
        /// <param name="mensagem">Conteudo</param>
        public static void NotificarAviso(this object fonte, string mensagem)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnAviso?.Invoke(type, mensagem, null);
            }
            else
            {
                OnAviso?.Invoke(fonte as Type, mensagem, null);
            }
        }

        /// <summary>
        /// Delegate padrao
        /// </summary>
        /// <param name="fonte">Tipo do objeto que esta distribuindo a mensagem</param>
        /// <param name="mensagem">Conteudo</param>
        /// <param name="exception">Excecao se houver</param>
        public static void NotificarErro(this object fonte, string mensagem, Exception exception)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnErro?.Invoke(type, mensagem, exception);
            }
            else
            {
                OnErro?.Invoke(fonte as Type, mensagem, exception);
            }
        }
        /// <summary>
        /// Registra um ouvinte para o evento atual
        /// </summary>
        /// <param name="ouvinte"></param>
        public static void RegistrarOuvinteInformacao(this Evento ouvinte)
        {
            OnInfo += ouvinte;
        }
        /// <summary>
        /// Registra um ouvinte para o evento atual
        /// </summary>
        /// <param name="ouvinte"></param>
        public static void RegistrarOuvinteAviso(this Evento ouvinte)
        {
            OnAviso += ouvinte;
        }
        /// <summary>
        /// Registra um ouvinte para o evento atual
        /// </summary>
        /// <param name="ouvinte"></param>
        public static void RegistrarOuvinteErro(this Evento ouvinte)
        {
            OnErro += ouvinte;
        }
    }
}
