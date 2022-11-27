using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Util.Objects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using static Propeus.Modulo.Configuracao.Constantes.Helper;

namespace Propeus.Modulo.Configuracao
{
    [Modulo]
    public class ConfiguracaoModulo : ModuloBase, IConfiguracaoModulo
    {
        public ConfiguracaoModulo(IGerenciador gerenciador, bool instanciaUnica = true, string caminhoConfiguracao = "config.pmc") : base(gerenciador, instanciaUnica)
        {
            CaminhoConfiguracao = caminhoConfiguracao;
        }

        public void CriarInstancia()
        {
            data = new Dictionary<string, string>();
            Carregar(CaminhoConfiguracao);
        }

        public void CriarConfiguracao()
        {


            if (this[ConfiguracaoAutoSalvamento].IsNull())
                this[ConfiguracaoAutoSalvamento] = true.ToString();
            if (this[ConfiguracaoTempoAtualizacao].IsNull())
                this[ConfiguracaoTempoAtualizacao] = 1000.ToString();

        }

        public string this[string configuracao] { get => Obter(configuracao); set => Inserir(configuracao, value); }

        public string CaminhoConfiguracao { get; }

        private Dictionary<string, string> data;

        public void Carregar(string caminhoConfiguracao)
        {
            if (File.Exists(caminhoConfiguracao))
            {
                using (FileStream fileStream = new FileStream(caminhoConfiguracao, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(data.GetType());
                    data = dataContractJsonSerializer.ReadObject(fileStream).To<Dictionary<string, string>>();
                    fileStream.Close();
                }
            }
            else
            {
                Salvar(caminhoConfiguracao);
            }
        }
        public void Salvar(string caminhoConfiguracao)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(data.GetType());

            using (MemoryStream memoryStream = new MemoryStream())
            {
                js.WriteObject(memoryStream, data);

                using (FileStream fileStream = new FileStream(caminhoConfiguracao, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    memoryStream.WriteTo(fileStream);
                    fileStream.Close();
                }
                memoryStream.Close();
            }

        }

        private void Inserir(string configuracao, string conteudo)
        {

            if (data.ContainsKey(configuracao))
            {
                data[configuracao] = conteudo;
            }
            else
            {
                data.Add(configuracao, conteudo);
            }

        }
        private string Obter(string configuracao)
        {
            if (data.ContainsKey(configuracao))
            {
                return data[configuracao];
            }
            else
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (bool.Parse(this[ConfiguracaoAutoSalvamento].ToString()))
                Salvar(CaminhoConfiguracao);

            base.Dispose(disposing);
        }
    }
}
