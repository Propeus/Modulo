using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.ManagerTests.Modulos
{
    [Module(Description = "Modulo para testar interface de contrato defeituoso", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    internal class PlantaModule : BaseModule, IPlantaContrato
    {
        public PlantaModule() { }

    }

    [ModuleContract("")]
    public interface IPlantaContrato : IModule
    {

    }

    [ModuleContract(default(Type))]
    public interface IVerduraContrato : IModule
    {

    }

    
    public interface IJogoContrato : IModule
    {

    }
}
