using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.ManagerTests.Contratos;

namespace Propeus.Module.ManagerTests.Modulos
{

    /// <summary>
    /// Modulo de teste simples
    /// 
    /// Instancia unica: SIM
    /// Auto inicializavel: NAO
    /// Auto atualizavel: NAO
    /// Manter vivo: NAO
    /// 
    /// Possui parametros: SIM
    /// Possui contratos: SIM
    /// Possui construtor: SIM
    /// </summary>
    [Module(Description = "Modulo de teste de instancia unica", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = true)]
    public class PessoaModule : BaseModule, IPessoaContrato
    {
        public PessoaModule() { }
        public PessoaModule(string nome, int idade)
        {
            Nome = nome;
            Idade = idade;
        }
        public PessoaModule(string nome, int idade, string? cpf, string genero = "a")
        {
            Nome = nome;
            Idade = idade;
            Cpf = cpf;
            Genero = genero;
        }

        public string Nome { get; }
        public int Idade { get; }
        public string? Cpf { get; }
        public string Genero { get; }
    }
}
