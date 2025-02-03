using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Taskjob.Modules;

using static Propeus.Module.Taskjob.Modules.TaskJobModule;

namespace Propeus.Module.Taskjob.Contracts
{
    [ModuleContract(typeof(TaskJobModule))]
    public interface ITaskJobContract : IModule
    {
        void RegisterJob(Action<TaskJobModule.BagAction> action, BagAction? bag = null, string? nomeJob = null);
        void RegisterRecurringJob(Action<TaskJobModule.BagAction> action, TimeSpan period, BagAction? bag = null, string nomeJob = null);
        string ToStringView();
        void UnregisterJob(string nomeJob);
        void WaitAll(TimeSpan? timeSpan = null);
    }
}