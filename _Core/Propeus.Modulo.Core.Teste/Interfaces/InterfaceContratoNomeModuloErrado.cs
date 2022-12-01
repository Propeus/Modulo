using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Teste.Interfaces
{
    [ModuloContrato("TesteModuloInexistente")]
    public interface InterfaceContratoNomeModuloErrado : IModulo
    {
    }
}
