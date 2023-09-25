﻿using System.Text;

using Propeus.Module.Abstract.Helpers;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Module.Abstract
{
    /// <summary>
    /// Classe base para o modulo
    /// </summary>
    public abstract class BaseModule : BaseModel, IModule
    {
        /// <summary>
        /// Inicializa um modulo
        /// </summary>
        protected BaseModule() : base()
        {
            Name = GetType().Name;
        }

        ///<inheritdoc/>
        public virtual void ConfigureModule()
        {
            State = State.Ready;
        }
        ///<inheritdoc/>
        public virtual void Launch()
        {
            State = State.Initialized;
        }

        /// <summary>
        /// Exibe informacoes basicas sobre o modulo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());

            _ = sb.AppendLine($"Nome: {Name}");
            if (GetType().GetModuleAttribute() != null)
            {
                _ = sb.AppendLine($"Descricao: {GetType().GetModuleAttribute()?.Description}");
            }

            return sb.ToString();
        }

    }
}
