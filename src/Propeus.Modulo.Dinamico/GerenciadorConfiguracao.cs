using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Modulo.Dinamico
{
    /// <summary>
    /// Classe de configuracao do <see cref="Gerenciador"/>
    /// </summary>
    public class GerenciadorConfiguracao
    {
        /// <summary>
        /// Construtor padrao
        /// </summary>
        public GerenciadorConfiguracao()
        {
            CarregamentoRapido = false;
            CaminhoArquivoModulos = "Modulos.path";
        }

        /// <summary>
        /// Pemite inicializar o gerenciador mais rapidamente, entretanto os novos modulos demorarao a ser encontrados
        /// </summary>
        /// <value>Por padrao o valor e <see langword="false"/></value>
        public bool CarregamentoRapido { get; set; }
        /// <summary>
        /// Caminho do arquivo que sera armazenado a lista de modulos mapeados
        /// </summary>
        /// <value>Lista de caminhos de modulos validos</value>
        public string CaminhoArquivoModulos { get; set; }
    }
}
