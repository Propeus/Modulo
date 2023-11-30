using Propeus.Module.IL.Core.Enums;

namespace Propeus.Module.IL.Core.Interfaces
{
    interface IILComponent
    {
        /// <summary>
        /// Nome do componente IL
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Tipo do componente IL
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// Modificadores de acesso
        /// <para>
        /// Para mais informações acesse este <see href="https://learn.microsoft.com/pt-br/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers">Manual</see>
        /// </para>
        /// </summary>
        Token[] ModifyAccess { get; }
    }
}