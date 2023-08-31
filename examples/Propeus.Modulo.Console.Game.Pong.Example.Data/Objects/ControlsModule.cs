using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Objects
{
    [Module(AutoUpdate = false, AutoStartable = false, Singleton = true)]
    public class ControlsModule : BaseModule
    {
        private CancellationTokenSource _cancelationTokenSouce;
        Task _controlPlayer;
        bool _exit;

        public event Action Up;
        public event Action Down;
        public event Action Exit;

        public ControlsModule() : base()
        {
            _exit = false;
            _cancelationTokenSouce = new CancellationTokenSource();
            _controlPlayer = Task.Run(ControlPlayer, _cancelationTokenSouce.Token);
        }

        private void ControlPlayer()
        {
            #region Update Player Paddle
            do
            {
                while (System.Console.KeyAvailable)
                {

                    switch (System.Console.ReadKey(intercept: true).Key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            Up?.Invoke();
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            Down?.Invoke();
                            break;
                        case ConsoleKey.Backspace:
                        case ConsoleKey.Spacebar:
                        case ConsoleKey.Escape:
                            Exit?.Invoke();
                            _exit = true;
                            break;
                    }
                }
            } while (_controlPlayer.Status == TaskStatus.Running && !_exit);

            #endregion
        }


        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _exit = true;
                try
                {
                    if (_controlPlayer.Status == TaskStatus.Running)
                    {
                        _cancelationTokenSouce?.Cancel();
                    }
                }
                catch (TaskCanceledException)
                {
                    //Jogo finalizado
                }
            }

            base.Dispose(disposing);
        }
    }
}
