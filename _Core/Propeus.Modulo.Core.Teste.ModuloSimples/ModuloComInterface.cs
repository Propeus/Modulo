using Propeus.Modulo.Core.Teste.ModuloSimples.Contratos;
using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Core.Teste.ModuloSimples
{


    [Modulo]
    public class ModuloComInterface : ModuloBase, IModuloComInterfaceContrato
    {
        public ModuloComInterface(IGerenciador gerenciador) : base(gerenciador, true)
        {

        }

        public bool teste { get; set; }
    }
}
