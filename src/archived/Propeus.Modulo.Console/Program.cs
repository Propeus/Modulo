using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
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
    public class Program : BaseModule
    {
        //Converter o Modulo.CLI em exe e dar um jeito de executar ele mesmo dentro de um gerenciador
        //#estouficandoloucosopode

        private readonly IModuleManager _moduleManager;

        private readonly CancellationTokenSource _cts;
        private ProcessStartInfo _processConsoleStartInfo;
        private Process _processConsole;
        private StreamWriter _inputConsoleProcess;
        private StreamReader _outputConsoleProcess;
        private Task _taskConsoleInputUser;

        /// <summary>
        /// Construtor padrao do <see cref="Program"/>
        /// </summary>
        /// <param name="moduleManager"></param>
        public Program(IModuleManager moduleManager) : base()
        {
            _moduleManager = moduleManager;
            _cts = new CancellationTokenSource();
        }

        static void Main(string[] args)
        {

            //using (IModuleManager moduleManager = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager())
            //{

            //    AppDomain.CurrentDomain.
            //}

        }

        ///<inheritdoc/>
        public override void ConfigureModule()
        {
            this._processConsoleStartInfo = new ProcessStartInfo();
            this._processConsoleStartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this._processConsoleStartInfo.UseShellExecute = false;
            this._processConsoleStartInfo.RedirectStandardInput = false;
            this._processConsoleStartInfo.RedirectStandardOutput = false;
            this._processConsoleStartInfo.CreateNoWindow = true;
            this._processConsoleStartInfo.FileName = "Module.exe";

            this._processConsole = new Process();
            this._processConsole.StartInfo = this._processConsoleStartInfo;


            base.ConfigureModule();
        }

        ///<inheritdoc/>
        public override void Launch()
        {
            this._processConsole.Start();
            //this._inputConsoleProcess = this._processConsole.StandardInput;
            //this._outputConsoleProcess = this._processConsole.StandardOutput;
            this._taskConsoleInputUser = Task.Run(CaptureInputUser, _cts.Token);
            base.Launch();
        }

        private void CaptureInputUser()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (_outputConsoleProcess != null)
                {
                    var sb = new StringBuilder();
                    while (!_outputConsoleProcess.EndOfStream)
                    {
                        sb.Append((char)_outputConsoleProcess.Read());
                    }

                    //moduloCliContrato.ExecutarCLI(sb.ToString().Split(' '));
                    sb.Clear();
                    Task.Delay(TimeSpan.FromSeconds(1));
                }
                else
                {
                    var input = System.Console.ReadLine();
                    //moduloCliContrato.ExecutarCLI(input.Split(' '));
                }

            }
        }
    }


}
