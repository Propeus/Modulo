namespace Propeus.Modulo.Teste.Modelos
{
    public class ClasseTesteComConstrutorSemInterface
    {
        public ClasseTesteComConstrutorSemInterface(string arg)
        {
            Arg = arg;
        }

        public string Arg { get; }

        public string Teste()
        {
            return Arg;
        }
    }
}
