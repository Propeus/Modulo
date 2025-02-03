using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Hosting.Contracts
{
    [ModuleContract("MessageQueueManagerModule")]
    public interface IMessageQueueManagerContract : IModule
    {
        void RegisterOnQueue(string queueName, Action<object> action);
        void SendData(string queueName, object data);
        void UnregisterOnQueue(string queueName, Action<object> action);
    }
}
