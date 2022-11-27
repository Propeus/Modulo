using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Core.Teste.ModuloSimples
{
    [Modulo]
    public class ModuloSemInterface : ModuloBase
    {
        public ModuloSemInterface(IGerenciador gerenciador) : base(gerenciador)
        {
        }
    }
}
