using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.Console.Contracts
{
    /// <summary>
    /// Interface de contrato para o Propeus.Modulo.CLI.CLIModulo
    /// </summary>
    [ModuleContract("CliModulo")]
    public interface IModuloCliContrato : IModule
    {
        /// <summary>
        /// Executa a CLI do Propeus.Modulo.CLI
        /// </summary>
        /// <param name="args"></param>
        void ExecutarCLI(string[] args);
    }

}
