namespace Propeus.Modulo.IL.Teste.Interfaces
{
    public interface IClasseComInterfaceNaoAssociado
    {
        string this[string teste] { get; }

        string Arg { get; }
        string Teste();
    }
}