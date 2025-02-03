using System;
using System.Diagnostics.CodeAnalysis;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.ManagerTests.Modulos
{
    /// <summary>
    /// Modulo de teste simples
    /// 
    /// Instancia unica: SIM
    /// Auto inicializavel: NAO
    /// Auto atualizavel: NAO
    /// Manter vivo: NAO
    /// 
    /// Possui parametros: NAO
    /// Possui contratos: NAO
    /// Possui construtor: NAO
    /// </summary>
    [Module(Description = "Modulo de teste de construtor ausente", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = true)]
    public class AnimalModule : BaseModule
    {
        [ExcludeFromCodeCoverage(Justification = "O programa nunca acessará o construtor de forma proposital")]
        private AnimalModule() { }
    }

    /// <summary>
    /// Modulo de teste simples que força um ID falso para forçar erro em teste unitario
    /// 
    /// Instancia unica: SIM
    /// Auto inicializavel: NAO
    /// Auto atualizavel: NAO
    /// Manter vivo: NAO
    /// 
    /// Possui parametros: NAO
    /// Possui contratos: NAO
    /// Possui construtor: SIM
    /// </summary>
    [Module(Description = "Modulo de teste de id invalido", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = true)]
    public class ModuloIdErradoModule : IModule
    {
        public ModuloIdErradoModule() { }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará esta propriedade")]
        public string Version { get; set; }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará esta propriedade")]
        public State State { get; set; }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará esta propriedade")]
        public string Name { get; set; }

        public string Id => Guid.NewGuid().ToString();

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará esta propriedade")]
        public string ManifestId { get; set; }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará este metodo")]
        public void ConfigureModule()
        {
            //Nao faz nada
        }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará este metodo")]
        public void Dispose()
        {
            //Nao faz nada
        }

        [ExcludeFromCodeCoverage(Justification = "O teste nunca acessará este metodo")]
        public void Launch()
        {
            //Nao faz nada
        }
    }
}
