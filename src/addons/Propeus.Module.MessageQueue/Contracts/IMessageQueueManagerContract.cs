using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.MessageQueue.Modules;

namespace Propeus.Module.MessageQueue.Contracts
{
    [ModuleContract(typeof(MessageQueueManagerModule))]
    public interface IMessageQueueManagerContract : IModule
    {
        void RegisterOnQueue(string queueName, Action<object> action);
        void SendData(string queueName, object data);
        void UnregisterOnQueue(string queueName, Action<object> action);
    }
}