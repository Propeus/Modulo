using System;

namespace Propeus.Modulo.IL.Geradores
{
    internal static partial class Constantes
    {
        public const string CONST_NME_PARAMETRO = "IL_Gerador_{0}_Parametro_";
        public static string GerarNomeParametro(string nomeMetodo)
        {
            return Constantes.GerarNome(string.Format(CONST_NME_PARAMETRO, nomeMetodo));
        }
    }

    internal class ILParametro
    {

        public ILParametro(string nomeMetodo
        , Type tipo
        , bool opcional = false
        , object defaultValue = null
        , string nome = Constantes.CONST_NME_PARAMETRO)
        {
            if (nome == Constantes.CONST_NME_PARAMETRO)
            {
                nome = Constantes.GerarNomeParametro(nomeMetodo);
            }
            tipo ??= typeof(object);

            Nome = nome;
            Tipo = tipo;
            Opcional = opcional;
            DefaultValue = defaultValue;
        }


        public Type Tipo { get; private set; }
        public bool Opcional { get; private set; }
        public object DefaultValue { get; private set; }
        public string Nome { get; private set; }
        public int Indice { get; internal set; }

        public static explicit operator Type(ILParametro obj)
        {
            return obj.ToType();
        }

        public Type ToType()
        {
            return Tipo;
        }
    }
}
