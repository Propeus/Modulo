using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.Configuracao.Teste
{
    [ModuloContrato("SimplesModulo")]
    public interface ISimplesModuloContrato : IModulo
    {
        bool Funcionou();
    }
}
