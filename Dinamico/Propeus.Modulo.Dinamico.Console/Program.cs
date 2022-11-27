using Propeus.Modulo.Modelos.Atributos;
using Propeus.Modulo.Modelos.Interfaces;
using System.Threading.Tasks;

namespace Propeus.Modulo.Dinamico.Console
{
    [ModuloContrato("DiagnosticoModulo")]
    public interface IDiagnosticoContrato : IModulo
    {
      
    }

    internal class Program
    {
        private static async Task Main()
        {
            Gerenciador gen = new Gerenciador(Core.Gerenciador.Atual);
          

            System.Console.WriteLine("Gerenciador iniciado");
            await gen.ManterVivoAsync();
            System.Console.WriteLine("Gerenciador finalizado");
        }

    
    }
}
