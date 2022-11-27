using Propeus.Modulo.Util.Teste.Interfaces;
using System.ComponentModel;

namespace Propeus.Modulo.Util.Teste.Modelo
{
    /// <summary>
    /// Classe simples para o teste unitario Propeus.NetCore.IL
    /// </summary>
    public class ClasseAjudaAtributoTeste : IInterfaceDeInterface
    {
        [Description("Atributo para verificação de teste")]
        public string Propriedade { get; set; }
    }
}