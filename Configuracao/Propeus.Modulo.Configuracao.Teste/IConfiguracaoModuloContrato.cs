using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.Configuracao.Teste
{
    [ModuloContrato("ConfiguracaoModulo")]
    public interface IConfiguracaoModuloContrato : IModulo
    {
        string this[string configuracao] { get; set; }

        string CaminhoConfiguracao { get; }

        void Carregar(string caminhoConfiguracao);
        void Salvar(string caminhoConfiguracao);
    }
}
