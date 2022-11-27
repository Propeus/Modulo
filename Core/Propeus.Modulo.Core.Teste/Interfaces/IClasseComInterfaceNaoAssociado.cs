namespace Propeus.Modulo.Teste.Interfaces
{
    public interface IClasseComInterfaceNaoAssociado
    {
        string this[string teste, bool mod = false] { get; set; }
        string Teste();
    }
}
