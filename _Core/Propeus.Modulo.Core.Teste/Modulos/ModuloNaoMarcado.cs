using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Teste.Modulos
{
    public class ModuloNaoMarcado : ModuloBase
    {
        public ModuloNaoMarcado(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }
    }
}
