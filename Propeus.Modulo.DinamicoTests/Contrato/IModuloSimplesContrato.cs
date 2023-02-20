using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.DinamicoTests.Contrato
{
    [ModuloContrato("ModuloSimples")]
    public interface IModuloSimplesContrato : IModulo
    {

        int Teste { get; set; }

        string Teste_String_GetSet { get; set; }
        double Teste_Double_GetSet { get; set; }
        float Teste_Float_GetSet { get; set; }
        decimal Teste_Decimal_GetSet { get; set; }
        bool Teste_Bool_GetSet { get; set; }

        string Teste_String_Get { get; }
        double Teste_Double_Get { get; }
        float Teste_Float_Get { get; }
        decimal Teste_Decimal_Get { get; }
        bool Teste_Bool_Get { get; }

        string Teste_String_GetSet_NotImplemented { get; set; }
        double Teste_Double_GetSet_NotImplemented { get; set; }
        float Teste_Float_GetSet_NotImplemented { get; set; }
        decimal Teste_Decimal_GetSet_NotImplemented { get; set; }
        bool Teste_Bool_GetSet_NotImplemented { get; set; }

        string Teste_String_Get_NotImplemented { get; }
        double Teste_Double_Get_NotImplemented { get; }
        float Teste_Float_Get_NotImplemented { get; }
        decimal Teste_Decimal_Get_NotImplemented { get; }
        bool Teste_Bool_Get_NotImplemented { get; }
    }
}
