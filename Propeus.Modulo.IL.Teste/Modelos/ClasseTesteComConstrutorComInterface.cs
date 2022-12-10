using Propeus.Modulo.IL.Teste.Interfaces;

namespace Propeus.Modulo.IL.Teste.Modelos
{
    public class ClasseTesteComConstrutorComInterface : IClasseComInterface
    {
        public ClasseTesteComConstrutorComInterface(string arg)
        {
            Arg = arg;
        }

        private string _ths;
        public string this[string teste] { get => _ths;set => _ths = value; }

        public string Arg { get; }

        public string TesteP { get; } = "Ok Props!";

        public string Teste()
        {
            return Arg;
        }
    }
}