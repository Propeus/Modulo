using System.ComponentModel;

namespace Propeus.Modulo.IL.Enums
{
    /// <summary>
    /// Enumerador para identificar o tipo de acessador de uma classe, metodo, propriedade etc.
    /// </summary>
    public enum TokenEnum
    {
        /// <summary>
        /// private
        /// </summary>
        [Description("Private")]
        Privado,

        /// <summary>
        /// public
        /// </summary>
        [Description("Public")]
        Publico,

        /// <summary>
        /// protected
        /// </summary>
        [Description("Family")]
        Protegido,

        /// <summary>
        /// internal
        /// </summary>
        [Description("Assembly")]
        Interno,

        /// <summary>
        /// virtual
        /// </summary>
        [Description("Virtual")]
        Virtual,

        /// <summary>
        /// final
        /// </summary>
        [Description("Final")]
        Final,

        /// <summary>
        /// newslot
        /// </summary>
        [Description("NewSlot")]
        NovoSlot,

        /// <summary>
        /// specialname
        /// </summary>
        [Description("SpecialName")]
        NomeEspecial,

        /// <summary>
        /// hidebysig
        /// </summary>
        [Description("HideBySig")]
        OcutarAssinatura,

        /// <summary>
        /// abstract
        /// </summary>
        [Description("Abstract")]
        Abstrato,

        /// <summary>
        /// class
        /// </summary>
        [Description("Class")]
        Classe,

        /// <summary>
        /// rtspecialname
        /// </summary>
        [Description("RTSpecialName")]
        RotuloNomeEspecial
    }
}