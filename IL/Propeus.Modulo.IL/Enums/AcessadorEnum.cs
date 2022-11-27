using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Propeus.Modulo.IL.Enums
{
    /// <summary>
    /// Acessadores de objetos
    /// </summary>
    public enum AcessadorEnum
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
        /// hidebysig
        /// </summary>

        [Description("HideBySig")]
        OcutarAssinatura
    }
}