using System.Diagnostics.CodeAnalysis;
using Propeus.Module.IL.Core.Enums;
using Propeus.Module.IL.Core.Interfaces;
using Propeus.Module.IL.Core.Proxy;

using static Propeus.Module.IL.Core.Proxy.ILBuilderProxy;

namespace Propeus.Module.IL.Core.Geradores.Components
{
    abstract class ILComponent : IILComponent
    {

        [SetsRequiredMembers]
        protected ILComponent(IILBuilderProxyScope iLBuilderProxy, Token[] modifyAccess, Type type, string name) => (BuilderProxy, ModifyAccess, Name, Type) = (iLBuilderProxy, modifyAccess, name, type);


        /// <summary>
        /// Classe de builders
        /// </summary>
        internal required IILBuilderProxyScope BuilderProxy { get; set; }
        ///<inheritdoc/>
        public required string Name { get; internal set; }
        ///<inheritdoc/>
        public required Type Type { get; internal set; }
        ///<inheritdoc/>
        public required Token[] ModifyAccess { get; internal set; }
    }
}