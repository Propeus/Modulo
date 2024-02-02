using Propeus.Module.Console.Playground.Contracts;

namespace Propeus.Module.Console.Playground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            /**
             * PLANO DE TESTE
             * 1 - Criar um arquivo zip contendo o modulo e manifesto
             * 2 - Iniciar o modulo de package
             * 3 - copiar o arquivo para a pasta de packages
             * 4 - Debugar o processo de carregamento, recarregamento e descarregamento do pacote e modulos
             * */
            using (var manager = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager())
            {
                using (var managerDynamic = Propeus.Module.Manager.Dynamic.ModuleManagerExtensions.CreateModuleManager(manager))
                {
                    var module = managerDynamic.CreateModule<IAssemblyLoadContextContract>();
                    var pathModule = Path.Combine(Utils.Utils.Helper.CURRENT_DIRECTORY, "");
                    module.RegisterAssemblyLoadContext(pathModule);
                    module.ExistsAssemblyLoadContext(pathModule);
                    module.GetAssemblyLoadContext(pathModule);
                    module.UnregisterAssemblyLoadContext(pathModule);
                }

            }
        }
    }
}
