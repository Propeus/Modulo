using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.Configuracao.SimplesModulo
{
    [Modulo]
    public class SimplesModulo : ModuloBase, ISimplesModulo
    {
        public SimplesModulo(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }

        public IConfiguracaoModuloContrato Configuracao { get; set; }

        public void CriarInstancia(IConfiguracaoModuloContrato config)
        {
            Configuracao = config;
        }

        public bool Funcionou()
        {
            return Configuracao != null;
        }
    }
}
