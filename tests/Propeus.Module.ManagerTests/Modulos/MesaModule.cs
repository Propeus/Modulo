using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.ManagerTests.Contratos;

namespace Propeus.Module.ManagerTests.Modulos
{
    [Module(Description = "Modulo para teste em atributos de contrato que possuem somente o nome", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    internal class MesaModule : BaseModule, IMesaContrato
    {

        public MesaModule()
        {

        }
    }
}
