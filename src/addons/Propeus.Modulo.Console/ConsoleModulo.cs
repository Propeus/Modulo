using System;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Modulo.Console.Contracts;

namespace Propeus.Modulo.Console
{

    //TODO: Caso o atributo possua AutoStartable, procurar o metodo Launch e anexar ao taskjob se estiver disponivel,
    //caso contrario inicar o launch de void e o usuario deve lidar com o ciclo de vida do modulo
    /// <summary>
    /// Exemplo de modulo auto inicializavel e funcional
    /// </summary>
    [Module(AutoStartable = true, Singleton = true)]
    public class ConsoleModulo : BaseModule
    {
      
        private readonly IModuleManager _moduleManager;

        /// <summary>
        /// Construtor padrao do <see cref="ConsoleModulo"/>
        /// </summary>
        /// <param name="moduleManager"></param>
        /// <param name="moduloCLI"></param>
        public ConsoleModulo(IModuleManager moduleManager) : base()
        {
            _moduleManager = moduleManager;
        }   

    }

}
