using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Console.Game.Pong.Example
{
    //https://github.com/dotnet/dotnet-console-games/blob/main/Projects/Pong


    [ModuloContrato("WindowModule")]
    public interface IWindowModuleContract : IModulo
    {
        Task Main();
    }


    internal class Program
    {


        static async Task Main(string[] args)
        {

            var gen = Dinamico.Gerenciador.Atual(Core.Gerenciador.Atual);
            if (!gen.Existe(typeof(IWindowModuleContract)))
            {
                await gen.Criar<IWindowModuleContract>().Main();
            }
            else
            {
                await gen.Obter<IWindowModuleContract>().Main();
            }
            await gen.ManterVivoAsync();

        }




    }
}