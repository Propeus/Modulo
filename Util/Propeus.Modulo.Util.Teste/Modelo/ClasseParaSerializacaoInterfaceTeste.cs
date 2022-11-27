using System;

namespace Propeus.Modulo.Util.Teste.Modelo
{
    [Serializable]
    public class ClasseParaSerializacaoInterfaceTeste
    {
        [NonSerialized]
        private string teste;

        public ClasseParaSerializacaoInterfaceTeste()
        {
        }

        public string Teste { get => teste; set => teste = value; }
    }
}