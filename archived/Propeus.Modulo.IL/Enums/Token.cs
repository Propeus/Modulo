using System.ComponentModel;

namespace Propeus.Modulo.IL.Enums
{
    /// <summary>
    /// Enumerador para identificar o tipo de acessador de uma classe, metodo, propriedade etc.
    /// </summary>
    public enum Token
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
        RotuloNomeEspecial,

        /// <summary>
        /// reuseslot
        /// </summary>
        [Description("ReuseSlot")]
        ReusoSlot,

        /// <summary>
        /// privateScope
        /// </summary>
        [Description("PrivateScope")]
        EscopoPrivado,

        /// <summary>
        /// vtablelayoutmask
        /// </summary>
        [Description("VtableLayoutMask")]
        VtableLayoutMask,

        /// <summary>
        /// AutoLayout
        /// </summary>
        [Description("AutoLayout")]
        Auto,

        /// <summary>
        /// Sealed
        /// </summary>
        [Description("Sealed")]
        Selado,

        /// <summary>
        /// AnsiClass
        /// </summary>
        [Description("AnsiClass")]
        Ansi,
        /// <summary>
        /// NestedPublic
        /// </summary>
        [Description("NestedPublic")]
        PublicaAninhado
    }
}