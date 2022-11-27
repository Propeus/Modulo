namespace Propeus.Modulo.IL.Teste.Interfaces
{
    public interface IClasseComInterfaceNaoAssociadoComEvento
    {
        string this[string teste, bool mod = false] { get; set; }

        string Teste();

        event tD TesteD;
    }
}