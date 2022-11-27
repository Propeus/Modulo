using Propeus.Modulo.IL.Teste.Interfaces;

namespace Propeus.Modulo.IL.Teste.Modelos
{
    public class ClasseTesteSemConstrutorComInterface : IClasseComInterface
    {
        public string this[string teste] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string TesteP { get; }
        public string Arg { get; }


        public string Teste()
        {
            return "Ok!";
        }
    }
}