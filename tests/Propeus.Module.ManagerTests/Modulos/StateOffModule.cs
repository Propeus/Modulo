using System;
using System.Diagnostics.CodeAnalysis;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.ManagerTests.Modulos
{
    [Module(Description = "Modulo para testar erro quando o estado é alterado forcadamente para 'off'", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    internal class StateOffModule : BaseModule
    {
        public StateOffModule() { }
        public void ForceStateOff()
        {
            this.State = State.Off;
        }
    }

    [Module(Description = "Classe para testar o caso quando é solicitado a instancia de um objeto que não herda de modulo", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    internal class ModuleNotImplemented
    {

    }

    //Modulo sem anotação de modulo
    internal class ModuleAttributeNotImplementedModule : BaseModule
    {
    }

    //Objeto que nao é classe e nem interface
    internal struct StrucIsNotModule : IModule
    {
        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public void ConfigureModule()
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public void Launch()
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public string Version { get; }
        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public State State { get; }
        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public string Name { get; }
        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public string Id { get; }
        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public string ManifestId { get; }

        [ExcludeFromCodeCoverage(Justification = Constantes.IMPLEMENTATION_CODE_COVERAGE_JUSTIFICATION)]
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
