using Propeus.Modulo.Util.Teste.Interfaces;

namespace Propeus.Modulo.Util.Teste.Modelo
{
    public struct StructTesteCast : IInterfaceTeste
    {
        public string Propriedade { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == GetType())
            {
                return ((StructTesteCast)(obj)).Propriedade == Propriedade;
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(StructTesteCast left, StructTesteCast right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StructTesteCast left, StructTesteCast right)
        {
            return !(left == right);
        }
    }
}