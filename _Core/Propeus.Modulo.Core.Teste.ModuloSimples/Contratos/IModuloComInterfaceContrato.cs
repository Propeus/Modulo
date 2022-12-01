using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Core.Teste.ModuloSimples.Contratos
{
    [ModuloContrato(typeof(ModuloComInterface))]
    public interface IModuloComInterfaceContrato : IModulo
    {
        bool teste { get; set; }
    }
}
