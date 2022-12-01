using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Teste.Modulos
{
    [Modulo]
    public class ModuloSemConstrutorPadrao : ModuloBase
    {
        public ModuloSemConstrutorPadrao(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }

        public ModuloSemConstrutorPadrao(int teste) : base(null, false)
        {

        }
    }
}
