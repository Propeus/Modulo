using Propeus.Modulo.Modelos;
using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;

namespace Propeus.Modulo.Dinamico.Teste.Modulo
{
    [Modulo]
    public class ModuloSimples : ModuloBase
    {
        public ModuloSimples(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
            Teste_String_Get="10";
            Teste_Double_Get=10.3;
            Teste_Float_Get=10.2f;
            Teste_Decimal_Get=10;
            Teste_Bool_Get=true;
        }

        public int Teste { get; set; }

        public string Teste_String_GetSet {get;set;}
        public double Teste_Double_GetSet {get;set;}
        public float Teste_Float_GetSet {get;set;}
        public decimal Teste_Decimal_GetSet {get;set;}
        public bool Teste_Bool_GetSet {get;set;}

        public string Teste_String_Get {get;}
        public double Teste_Double_Get {get;}
        public float Teste_Float_Get {get;}
        public decimal Teste_Decimal_Get {get;}
        public bool Teste_Bool_Get {get;}
    }
}
