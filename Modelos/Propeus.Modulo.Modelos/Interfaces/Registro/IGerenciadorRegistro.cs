using System;
using System.Collections.Generic;
using System.Text;

namespace Propeus.Modulo.Modelos.Interfaces.Registro
{
    /// <summary>
    /// Modelo base para criação de gerenciadores
    /// </summary>
    public interface IGerenciadorRegistro
    {
        /// <summary>
        /// Registra o modulo no gerenciador
        /// <para>Caso use o <see cref="ModuloBase"/>, não será necessário o uso desta função.</para>
        /// </summary>
        /// <param name="modulo"></param>
        void Registrar(IModulo modulo);
    }
}

