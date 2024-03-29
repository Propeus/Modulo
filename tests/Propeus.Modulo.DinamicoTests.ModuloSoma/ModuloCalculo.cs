﻿using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.DinamicoTests.ModuloSoma
{
    [ModuloContrato("ModuloCalculo")]
    public interface ModuloCalculoContrato: IModulo
    {
        int Calcular(int a, int b);
    }

    [Modulo]
    public class ModuloCalculo : ModuloBase
    {
        public ModuloCalculo() : base()
        {
        }

        public int Calcular(int a, int b)
        {
            return a + b;
        }
    }
}