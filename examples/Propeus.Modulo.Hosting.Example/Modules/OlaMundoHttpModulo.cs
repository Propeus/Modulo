using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Modulos;

namespace Propeus.Modulo.Hosting.Example.Modules
{
    [Modulo]
    public class OlaMundoHttpModulo : ModuloBase
    {

        public string Executar()
        {
            return $"Ola mundo, modulo {Id}";
        }

    }


    public class Teste
    {
        private readonly IServiceCollection serviceDescriptors;

        public Teste(IServiceCollection serviceDescriptors)
        {
            this.serviceDescriptors = serviceDescriptors;
        }

        public void Add(Type type)
        {
            serviceDescriptors.AddScoped(type);
        }
    }
}
