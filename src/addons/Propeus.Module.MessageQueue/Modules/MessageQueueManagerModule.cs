using System.Collections.Concurrent;
using System.Reflection;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.MessageQueue.Contracts;

namespace Propeus.Module.MessageQueue.Modules
{
    /// <summary>
    /// Modulo para configurar mensagira por evento
    /// </summary>
    [Module(Description = "Modulo para gerenciar filas de mensageria", Singleton = true, AutoStartable = false, AutoUpdate = false)]
    public class MessageQueueManagerModule : BaseModule, IMessageQueueManagerContract
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, Action<object>>> Queues = new ConcurrentDictionary<string, ConcurrentDictionary<string, Action<object>>>();

        /// <summary>
        /// Cria uma conexão entre o emissor e receptor da mensagem
        /// </summary>
        /// <param name="queueName">Nome da fila</param>
        /// <param name="action">Invoker</param>
        public void RegisterOnQueue(string queueName, Action<object> action)
        {

            string hashBase64 = GetHashAction(action);
            if (!Queues.ContainsKey(queueName))
            {
                Queues.TryAdd(queueName, new ConcurrentDictionary<string, Action<object>>());
            }

            if (!Queues[queueName].ContainsKey(hashBase64))
            {
                Queues[queueName].TryAdd(hashBase64, action);
            }
        }

        /// <summary>
        /// Remove a conexao entre o receptor e o emissoe da mensagem
        /// </summary>
        /// <param name="queueName">Nome da fila</param>
        /// <param name="action">Invoker a ser removido</param>
        public void UnregisterOnQueue(string queueName, Action<object> action)
        {
            string hashBase64 = GetHashAction(action);
            if (Queues.TryGetValue(queueName, out ConcurrentDictionary<string, Action<object>>? value))
            {
                if (value.ContainsKey(hashBase64))
                {
                    value.TryRemove(hashBase64, out _);
                }

                if (value.IsEmpty)
                {
                    Queues.TryRemove(queueName, out _);
                }

            }

        }

        /// <summary>
        /// Envia a mensagem para a fila desejado
        /// </summary>
        /// <param name="queueName">Nome da fila</param>
        /// <param name="data">Dados a serem transmitidos</param>
        public void SendData(string queueName, object data)
        {
            if (Queues.ContainsKey(queueName))
            {
                foreach (var item in Queues[queueName])
                {
                    item.Value.Invoke(data);
                }
            }
        }

        private string GetHashAction(Action<object> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var ilByte = action.GetMethodInfo().GetMethodBody().GetILAsByteArray();
            string hashBase64 = "";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(ilByte);
                hashBase64 = Convert.ToBase64String(hash);
                return hashBase64;
            }
        }
    }
}
