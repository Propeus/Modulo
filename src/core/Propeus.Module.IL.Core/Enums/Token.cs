using System.ComponentModel;

namespace Propeus.Module.IL.Core.Enums
{

    /**
     * MINI MANUAL
     * Este enum serve para agrupar todos os identificadores do IL em um unico lugar, permitindo a padronização durante a construção de um tipo com mais facilidade
     * 
     * TODOs
     * TODO: Adicionar todos os links de documentação para cada item do enum
     * **/

    /// <summary>
    /// Enumerador para identificar o tipo de acessador de uma classe, metodo, propriedade etc.
    /// </summary>
    public enum Token
    {
        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/private">private</see>
        /// </summary>
        [Description("Private")]
        Privado,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/public">public</see>
        /// </summary>
        [Description("Public")]
        Publico,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected">protected</see>
        /// </summary>
        [Description("Family")]
        Protegido,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/internal">internal</see>
        /// </summary>
        [Description("Assembly")]
        Interno,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual">virtual</see>
        /// </summary>
        [Description("Virtual")]
        Virtual,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed">sealed</see>
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
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract">abstract</see>
        /// </summary>
        [Description("Abstract")]
        Abstrato,

        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/class">class</see>
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
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed">sealed</see>
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
        PublicaAninhado,
        /// <summary>
        /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static">static</see>
        /// </summary>
        [Description("Static")]
        Estatico
    }
}