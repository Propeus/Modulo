using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Game.Pong.Example
{
    //https://github.com/dotnet/dotnet-console-games/blob/main/Projects/Pong


    [ModuleContract("WindowModule")]
    public interface IWindowModuleContract : IModule
    {
        Task Main();
    }


    internal class Program
    {
        private static async Task Main(string[] args)
        {

            IModuleManager gen = Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager());
            if (!gen.ExistsModule(typeof(IWindowModuleContract)))
            {
                await gen.CreateModule<IWindowModuleContract>().Main();
            }
            else
            {
                await gen.GetModule<IWindowModuleContract>().Main();
            }

        }




    }
}