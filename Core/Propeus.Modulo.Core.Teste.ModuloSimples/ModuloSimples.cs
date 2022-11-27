using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Core.Teste.ModuloSimples
{
    [Modulo]
    public class ModuloSimples : ModuloBase
    {
        public ModuloSimples(IGerenciador gerenciador) : base(gerenciador)
        {
        }

        public IGerenciador Contrato { get; private set; }

        public void CriarInstancia(IGerenciador contrato)
        {
            Contrato = contrato;
        }

    }

    [ModuloContrato("ModuloSimples")]
    public interface IModuloSimplesContrato : IModulo
    {

    }

}
