using System;

namespace Propeus.Modulo.Util.Teste.Modelo
{
    /// <summary>
    /// Esta classe foi criado somente para fins de teste.
    /// <para>Não foi aplicado nenhuma otimização ou correção de codigo ou melhores praticas por motivos de testes.</para>
    /// </summary>
    public class ClasseHeranca : ClasseAjudaAtributoTeste
    {
        private int teste;

#pragma warning disable CS0067 // O evento "ClasseHeranca.Evento" nunca é usado

        public event EventHandler Evento;

#pragma warning restore CS0067 // O evento "ClasseHeranca.Evento" nunca é usado

        public string Str { get; private set; }

        public ClasseHeranca()
        {
        }

        public ClasseHeranca(int teste)
        {
            this.teste = teste;
        }

        public int this[int indice] { get => teste; set { } }

        public void Teste(int teste)
        {
            this.teste = teste;
        }

        public void TesteSemRetorno()
        {
            Console.WriteLine("TesteSemRetorno - OK - " + teste);
        }

        public int TesteMetodoRetorno(int a)
        {
            teste = a;
            return teste;
        }

        public void TesteMetodoParametroOverload()
        {
            teste = 0;
        }

        public void TesteMetodoParametroOverload(int a)
        {
            teste = a;
        }

        public void TesteMetodoParametroOverload(int a, int b)
        {
            teste = a;
            teste = b;
        }

        public void TesteMetodoParametroOverload(int a, int b, int c)
        {
            teste = a;
            teste = b;
            teste = c;
        }

        public void TesteMetodoParametroTipoDiferentes(int a, string b, int c)
        {
            teste = a;
            Str = b;
            teste = c;
        }

        public void TesteMetodoParametroTipoDiferentes(int a, int b, string c)
        {
            teste = a;
            teste = b;
            Str = c;
        }
    }
}