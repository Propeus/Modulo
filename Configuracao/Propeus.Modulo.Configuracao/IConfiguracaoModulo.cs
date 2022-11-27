namespace Propeus.Modulo.Configuracao
{
    public interface IConfiguracaoModulo
    {
        string this[string configuracao] { get; set; }

        string CaminhoConfiguracao { get; }

        void Carregar(string caminhoConfiguracao);
        void Salvar(string caminhoConfiguracao);
    }
}