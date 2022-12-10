namespace Propeus.Modulo.IL.Teste.Interfaces
{
    public interface IClasseComInterface
    {
        string TesteP { get; }
        string Arg { get; }
        string this[string teste] { get ; set ; }

        string Teste();
    }
}