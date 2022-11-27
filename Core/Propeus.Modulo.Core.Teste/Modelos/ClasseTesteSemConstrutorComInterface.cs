using Propeus.Modulo.Teste.Interfaces;

namespace Propeus.Modulo.Teste.Modelos
{
    public class ClasseTesteSemConstrutorComInterface : IClasseComInterface
    {
        public string TesteP { get; }

        public string Teste()
        {
            return "Ok!";
        }
    }
}
