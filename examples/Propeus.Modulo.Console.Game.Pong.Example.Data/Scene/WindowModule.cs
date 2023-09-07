using System.Diagnostics;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Objects;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Scene
{
    [Module(AutoUpdate = true, AutoStartable = false, Singleton = true)]
    public class WindowModule : BaseModule
    {
        private readonly IModuleManager gerenciador;
        private readonly float multiplier;



        public WindowModule(IModuleManager gerenciador, int? width, int? height, ControlsModule controlsModule) : base()
        {
            this.gerenciador = gerenciador;
            multiplier = 1.1f;


            Width = width ?? System.Console.WindowWidth;
            Height = height ?? System.Console.WindowHeight;

            _controlsModule = controlsModule;
            _controlsModule.Up += _controlsModule_Up;
            _controlsModule.Down += _controlsModule_Down;
            _controlsModule.Exit += _controlsModule_Exit;


            enemyStopwatch = new Stopwatch();
            enemyInputDelay = TimeSpan.FromMilliseconds(100);
            delay = TimeSpan.FromMilliseconds(10);
            flg_loop = true;
            System.Console.CursorVisible = false;


            PaddleA = this.gerenciador.CreateModule<PaddleModule>(new object[] { Height });
            PaddleA.ScoreEvent += PaddleA_ScoreEvent;
            PaddleB = this.gerenciador.CreateModule<PaddleModule>(new object[] { Height });
            PaddleB.ScoreEvent += PaddleB_ScoreEvent;

            BallModule = this.gerenciador.CreateModule<BallModule>(new object[] { Width, Height });

        }

        private void PaddleB_ScoreEvent(int obj)
        {
            flg_loop = false;
        }

        private void PaddleA_ScoreEvent(int obj)
        {
            flg_loop = false;

        }

        private void _controlsModule_Exit()
        {
            System.Console.Clear();
            System.Console.Write("Pong was closed.");
        }

        private void _controlsModule_Down()
        {
            PaddleA.Paddle = Math.Min(PaddleA.Paddle + 1, Height - PaddleA.PaddleSize - 1);
        }

        private void _controlsModule_Up()
        {
            PaddleA.Paddle = Math.Max(PaddleA.Paddle - 1, 0);
        }

        public int Width { get; set; }
        public int Height { get; set; }

        private readonly Stopwatch enemyStopwatch;
        private readonly TimeSpan enemyInputDelay;
        private readonly TimeSpan delay;
        private bool flg_loop;

        #region Objects

        private readonly PaddleModule PaddleA;
        private readonly PaddleModule PaddleB;
        private readonly ControlsModule _controlsModule;
        private BallModule BallModule;

        private RenderModule RenderModule
        {
            get
            {
                if (gerenciador.ExistsModule(typeof(RenderModule)))
                {
                    return gerenciador.GetModule<RenderModule>();
                }
                else
                {

                    var render = gerenciador.CreateModule<RenderModule>();
                    gerenciador.KeepAliveModule(render);
                    return render;
                }

            }
        }
        #endregion

        public async Task Main()
        {
            RenderModule.Clear();
            BallModule.StopWatch.Restart();
            enemyStopwatch.Restart();
            while (PaddleA.Score < 3 && PaddleB.Score < 3)
            {
                while (flg_loop)
                {
                    Update(PaddleA, PaddleB);
                    BallModule.StopWatch.Restart();
                    await Task.Delay(delay);
                }

                RenderModule.RenderBalls(BallModule, true);

                flg_loop = true;

                gerenciador.RemoveModule(BallModule);
                BallModule = gerenciador.CreateModule<BallModule>(new object[] { Width, Height });
            }
            RenderModule.Clear();
            if (PaddleA.Score > PaddleB.Score)
            {
                System.Console.Write("You win.");
            }
            if (PaddleA.Score < PaddleB.Score)
            {
                System.Console.Write("You lose.");
            }
        }

        public void Update(PaddleModule paddleA, PaddleModule paddleB)
        {
            UpdateBall(paddleA, paddleB);
            UpdateComputerPaddle(paddleB);
            RenderModule.RenderPaddles(paddleA, paddleB, Height, Width);

        }

        private void UpdateBall(PaddleModule paddleA, PaddleModule paddleB)
        {

            float time = (float)BallModule.StopWatch.Elapsed.TotalSeconds * 15;

            #region Update Ball

            // Collision With Paddle A
            paddleA.CalculateColisionBall(BallModule, false, Height, Width, multiplier, time);

            // Collision With Paddle B
            paddleB.CalculateColisionBall(BallModule, true, Height, Width, multiplier, time);

            if (flg_loop)
            {
                // Updating Ball Position
                RenderModule.RenderBalls(BallModule, true);
                BallModule.UpdateBallNewPosition(time);
                RenderModule.RenderBalls(BallModule, false);
            }

            #endregion
        }

        private void UpdateComputerPaddle(PaddleModule paddleB)
        {
            #region Update Computer Paddle

            if (enemyStopwatch.Elapsed > enemyInputDelay)
            {
                if (BallModule.Y < paddleB.Paddle + (paddleB.PaddleSize / 2) && BallModule.DY < 0)
                {
                    paddleB.Paddle = Math.Max(paddleB.Paddle - 1, 0);
                }
                else if (BallModule.Y > paddleB.Paddle + (paddleB.PaddleSize / 2) && BallModule.DY > 0)
                {
                    paddleB.Paddle = Math.Min(paddleB.Paddle + 1, Height - paddleB.PaddleSize - 1);
                }
                enemyStopwatch.Restart();
            }

            #endregion
        }


        private float GetLineValue(((float X, float Y) A, (float X, float Y) B) line, float x)
        {
            // order points from least to greatest X
            if (line.B.X < line.A.X)
            {
                (line.A, line.B) = (line.B, line.A);
            }
            // find the slope
            float slope = (line.B.Y - line.A.Y) / (line.B.X - line.A.X);
            // find the y-intercept
            float yIntercept = line.A.Y - (line.A.X * slope);
            // find the function's value at parameter "x"
            return (x * slope) + yIntercept;
        }
    }
}
