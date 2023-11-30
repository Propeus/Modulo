using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Manager.Dynamic.Package.Modules
{


    [Module(Description = "Gerenciador de pacotes para modulo", AutoStartable = false, AutoUpdate = false, KeepAlive = true, Singleton = true)]
    public class ModuleManager : BaseModule, IModuleManager
    {
        private readonly IModuleManager _moduleManager;
        private readonly string _extPackage;

        public ModuleManager(IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
            //Propeus Modulo Pacote (PMP)
            _extPackage = ".pmp";
            /**
             * Criar um gerenciador de modulo que faça...:
             * 1 - Olhe e monitore todos os arquivos .mde
             * 2 - Carregue todos os arquivos contidos la dentro quando for necessario
             * */
        }

        public DateTime StartDate => _moduleManager.StartDate;

        public DateTime LastUpdate => _moduleManager.LastUpdate;

        public int InitializedModules => _moduleManager.InitializedModules;

        public T CreateModule<T>() where T : IModule
        {
            return _moduleManager.CreateModule<T>();
        }

        public IModule CreateModule(Type moduleType)
        {
            return _moduleManager.CreateModule(moduleType);
        }

        public IModule CreateModule(string moduleName)
        {
            return _moduleManager.CreateModule(moduleName);
        }

        public void RemoveModule(string idModule)
        {
            _moduleManager.RemoveModule(idModule);
        }

        public void RemoveModule<T>(T moduleInstance) where T : IModule
        {
            _moduleManager.RemoveModule(moduleInstance);
        }



        public T GetModule<T>() where T : IModule
        {
            return _moduleManager.GetModule<T>();
        }

        public IModule GetModule(Type moduleType)
        {
            return _moduleManager.GetModule(moduleType);
        }

        public IModule GetModule(string idModule)
        {
            return _moduleManager.GetModule(idModule);
        }

        public bool ExistsModule(IModule moduleInstance)
        {
            return _moduleManager.ExistsModule(moduleInstance);
        }

        public bool ExistsModule(Type moduleType)
        {
            return _moduleManager.ExistsModule(moduleType);
        }

        public bool ExistsModule(string idModule)
        {
            return _moduleManager.ExistsModule(idModule);
        }

        public T RecycleModule<T>(T moduleInstance) where T : IModule
        {
            return _moduleManager.RecycleModule(moduleInstance);
        }

        public IModule RecycleModule(string idModule)
        {
            return _moduleManager.RecycleModule(idModule);
        }

        public IEnumerable<IModule> ListAllModules()
        {
            return _moduleManager.ListAllModules();
        }

        public void KeepAliveModule(IModule moduleInstance)
        {
            _moduleManager.KeepAliveModule(moduleInstance);
        }

        public T CreateModule<T>(object[] args) where T : IModule
        {
            return _moduleManager.CreateModule<T>(args);
        }

        public IModule CreateModule(Type moduleType, object[] args)
        {
            return _moduleManager.CreateModule(moduleType, args);
        }

        public IModule CreateModule(string moduleName, object[] args)
        {
            return _moduleManager.CreateModule(moduleName, args);
        }
    }
}
