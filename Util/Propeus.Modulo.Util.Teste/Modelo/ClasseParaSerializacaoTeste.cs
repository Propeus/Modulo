using System;

namespace Propeus.Modulo.Util.Teste.Modelo
{
    [Serializable]
    public class ClasseParaSerializacaoTeste
    {
        [NonSerialized]
        private string teste;

        public ClasseParaSerializacaoTeste()
        {
        }

        public string Teste { get => teste; set => teste = value; }
    }
}