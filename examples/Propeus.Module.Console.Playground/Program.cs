using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Console.Playground.Contracts;
using Propeus.Module.Manager.Dynamic;
using Propeus.Module.Package.Modules;
using System.Runtime.Loader;

namespace Propeus.Module.Console.Playground
{
    #region testes
    [Module.Abstract.Attributes.Module(Description = "Modulo de teste", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = true)]
    public class Calculaora : BaseModule
    {

        public Calculaora() { }
        public int Somar(int x, int y)
        {
            return x + y;
        }

    }

    [ModuleContract("Calculaora")]
    public interface ICalculaoraContrato : IModule
    {
        public int Somar(int x, int y);
    }

    [ModuleContract("Calculaora")]
    public interface ICalculaoraContrato2 : IModule
    {
        public int Somar(int x, int y);
    }
    [Module]
    class DependenciaA : BaseModule
    {
        public DependenciaA(ICalculaoraContrato calculaora)
        {
            Calculaora = calculaora;
        }

        public ICalculaoraContrato Calculaora { get; }

        protected override void Dispose(bool disposing)
        {
            Calculaora.Dispose();
            base.Dispose(disposing);
        }
    }
    [Module]
    class DependenciaB : BaseModule
    {
        public DependenciaB(ICalculaoraContrato2 calculaora)
        {
            Calculaora = calculaora;
        }

        public ICalculaoraContrato2 Calculaora { get; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Calculaora.Dispose();
        }
    }


    [ModuleContract("AssemblyLoadContextModule")]
    public interface IAssemblyLoadContextContractB : IModule
    {
        bool ExistsAssemblyLoadContext(string FullPathAssembly);
        AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly);
        bool RegisterAssemblyLoadContext(string FullPathAssembly);
        bool UnregisterAssemblyLoadContext(string FullPathAssembly);
    }

    [ModuleContract("AssemblyLoadContextModule")]
    public interface IAssemblyLoadContextContractC : IModule
    {
        bool ExistsAssemblyLoadContext(string FullPathAssembly);
        AssemblyLoadContext GetAssemblyLoadContext(string FullPathAssembly);
        bool RegisterAssemblyLoadContext(string FullPathAssembly);
        bool UnregisterAssemblyLoadContext(string FullPathAssembly);
    }
    #endregion

    internal class Program
    {
      


        static void Main(string[] args)
        {
            //System.Console.WriteLine(Propeus.Module.Utils.Utils.Helper.GetCurrentTargetFramework());
            //return;
            using (var manager = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager())
            {
                using (var managerDynamic = ModuleManagerExtensions.CreateModuleManager(manager))
                {
                    //var a1 = managerDynamic.CreateModule<Calculaora>();
                    ////ICalculaoraContrato a22 = managerDynamic.CreateModule<ICalculaoraContrato>();
                    //var a2 = managerDynamic.CreateModule<DependenciaA>();
                    //var a3 = managerDynamic.CreateModule<DependenciaB>();

                    var module = managerDynamic.CreateModule<IAssemblyLoadContextContract>();
                    var moduleA = managerDynamic.CreateModule<IAssemblyLoadContextContractB>();
                    var moduleB = managerDynamic.CreateModule<IAssemblyLoadContextContractC>();

                    NugetPackageLoader nugetPackageLoader = managerDynamic.CreateModule<NugetPackageLoader>();
                    nugetPackageLoader.Load("Newtonsoft.Json", "11.0.2");
                    nugetPackageLoader.Load("Newtonsoft.Json", "13.0.3");
                }
            }
            return;

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
                using (var managerDynamic = ModuleManagerExtensions.CreateModuleManager(manager))
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
