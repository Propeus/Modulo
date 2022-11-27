using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Dinamico.Teste.Modulo
{
    [Modulo]
    public class ModuloInstanciaUnica : ModuloBase
    {
        public ModuloInstanciaUnica(IGerenciador gerenciador, bool instanciaUnica = true) : base(gerenciador, instanciaUnica)
        {
        }
    }
}
