using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Abstrato.Proveders
{
    /// <summary>
    /// Classe para disparar mensagens para os ouvintes
    /// </summary>
    public static class EventoProvider
    {

        public delegate void Evento(Type fonte, string mensagem, Exception exception);

        static event Evento OnInfo;
        static event Evento OnAviso;
        static event Evento OnErro;

        public static void NotificarInformacao(this object fonte, string mensagem)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnInfo?.Invoke(type, mensagem, null);
            }
            else
            {
                OnInfo?.Invoke((fonte as Type), mensagem, null);
            }
        }
        public static void NotificarAviso(this object fonte, string mensagem)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnAviso?.Invoke(type, mensagem, null);
            }
            else
            {
                OnAviso?.Invoke((fonte as Type), mensagem, null);
            }
        }
        public static void NotificarErro(this object fonte, string mensagem,Exception exception)
        {
            Type type = fonte.GetType();
            if (type != typeof(Type))
            {
                OnErro?.Invoke(type, mensagem, exception);
            }
            else
            {
                OnErro?.Invoke((fonte as Type), mensagem, exception);
            }
        }

        public static void RegistrarOuvinteInformacao(this Evento ouvinte)
        {
            OnInfo += ouvinte;
        }
        public static void RegistrarOuvinteAviso(this Evento ouvinte)
        {
            OnAviso += ouvinte;
        }
        public static void RegistrarOuvinteErro(this Evento ouvinte)
        {
            OnErro += ouvinte;
        }
    }
}
