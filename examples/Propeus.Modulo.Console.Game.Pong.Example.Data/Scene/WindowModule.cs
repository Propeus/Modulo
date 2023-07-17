using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Objects;

using static System.Formats.Asn1.AsnWriter;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Scene
{
    [Module(AutoUpdate = true, AutoStartable = false)]
    public class WindowModule : BaseModule
    {
        private readonly IModuleManagerArguments gerenciador;
        private readonly float multiplier;

        public WindowModule(IModuleManager gerenciador) : this(gerenciador, null, null)
        {

        }

        public WindowModule(IModuleManager gerenciador, int? width, int? height) : base(true)
        {
            this.gerenciador = gerenciador as IModuleManagerArguments;
            multiplier = 1.1f;


            Width = width ?? System.Console.WindowWidth;
            Height = height ?? System.Console.WindowHeight;
            Stopwatch = new Stopwatch();
            enemyStopwatch = new Stopwatch();
            enemyInputDelay = TimeSpan.FromMilliseconds(100);
            delay = TimeSpan.FromMilliseconds(10);
            flg_loop = true;
            System.Console.CursorVisible = false;


            PaddleA = this.gerenciador.CreateModule<PaddleModule>(new object[] { Height });
            PaddleB = this.gerenciador.CreateModule<PaddleModule>(new object[] { Height });
            BallModule = this.gerenciador.CreateModule<BallModule>(new object[] { Width, Height });
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public Stopwatch Stopwatch { get; }
        private readonly Stopwatch enemyStopwatch;
        private readonly TimeSpan enemyInputDelay;
        private readonly TimeSpan delay;
        private bool flg_loop;

        #region Objects

        private readonly PaddleModule PaddleA;
        private readonly PaddleModule PaddleB;
        private BallModule BallModule;

        RenderModule RenderModule
        {
            get
            {
                if (gerenciador.ExistsModule(typeof(RenderModule)))
                    return gerenciador.GetModule<RenderModule>();

                return gerenciador.CreateModule<RenderModule>();
            }
        }
        #endregion

        public async Task Main()
        {
            System.Console.Clear();
            Stopwatch.Restart();
            enemyStopwatch.Restart();
            while (PaddleA.Score < 3 && PaddleB.Score < 3)
            {
                while (flg_loop)
                {
                    Update(PaddleA, PaddleB);
                    Stopwatch.Restart();
                    await Task.Delay(delay);
                }


                System.Console.SetCursorPosition((int)BallModule.X, (int)BallModule.Y);
                System.Console.Write(' ');

                flg_loop = true;

                gerenciador.RemoveModule(BallModule);
                BallModule = this.gerenciador.CreateModule<BallModule>(new object[] { Width, Height });
            }
            System.Console.Clear();
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
            UpdatePlayerPaddle(paddleA);
            UpdateComputerPaddle(paddleB);
            RenderModule.RenderPaddles(paddleA, paddleB, Height, Width);

        }

        private void UpdateBall(PaddleModule paddleA, PaddleModule paddleB)
        {
            #region Update Ball

            // Compute Time And New Ball Position
            float time = (float)Stopwatch.Elapsed.TotalSeconds * 15;
            var (X2, Y2) = (BallModule.X + time * BallModule.DX, BallModule.Y + time * BallModule.DY);

            // Collisions With Up/Down Walls
            if (Y2 < 0 || Y2 > Height)
            {
                BallModule.DY = -BallModule.DY;
                Y2 = BallModule.Y + BallModule.DY;
            }

            // Collision With Paddle A
            if (Math.Min(BallModule.X, X2) <= 2 && 2 <= Math.Max(BallModule.X, X2))
            {
                int ballPathAtPaddleA = Height - (int)GetLineValue(((BallModule.X, Height - BallModule.Y), (X2, Height - Y2)), 2);
                ballPathAtPaddleA = Math.Max(0, ballPathAtPaddleA);
                ballPathAtPaddleA = Math.Min(Height - 1, ballPathAtPaddleA);
                if (paddleA.Paddle <= ballPathAtPaddleA && ballPathAtPaddleA <= paddleA.Paddle + paddleA.PaddleSize)
                {
                    BallModule.DX = -BallModule.DX;
                    BallModule.DX *= multiplier;
                    BallModule.DY *= multiplier;
                    X2 = BallModule.X + time * BallModule.DX;
                }
            }

            // Collision With Paddle B
            if (Math.Min(BallModule.X, X2) <= Width - 2 && Width - 2 <= Math.Max(BallModule.X, X2))
            {
                int ballPathAtPaddleB = Height - (int)GetLineValue(((BallModule.X, Height - BallModule.Y), (X2, Height - Y2)), Width - 2);
                ballPathAtPaddleB = Math.Max(0, ballPathAtPaddleB);
                ballPathAtPaddleB = Math.Min(Height - 1, ballPathAtPaddleB);
                if (paddleB.Paddle <= ballPathAtPaddleB && ballPathAtPaddleB <= paddleB.Paddle + paddleB.PaddleSize)
                {
                    BallModule.DX = -BallModule.DX;
                    BallModule.DX *= multiplier;
                    BallModule.DY *= multiplier;
                    X2 = BallModule.X + time * BallModule.DX;
                }
            }

            // Collisions With Left/Right Walls
            if (X2 < 0)
            {
                paddleB.Score++;
                flg_loop = false;
                return;
            }
            if (X2 > Width)
            {
                paddleA.Score++;
                flg_loop = false;
                return;
            }

            // Updating Ball Position
            System.Console.SetCursorPosition((int)BallModule.X, (int)BallModule.Y);
            System.Console.Write(' ');
            BallModule.X += time * BallModule.DX;
            BallModule.Y += time * BallModule.DY;
            System.Console.SetCursorPosition((int)BallModule.X, (int)BallModule.Y);
            System.Console.Write('O');

            #endregion
        }
        private void UpdatePlayerPaddle(PaddleModule paddleA)
        {
            #region Update Player Paddle

            if (System.Console.KeyAvailable)
            {
                switch (System.Console.ReadKey(intercept: true).Key)
                {
                    case ConsoleKey.UpArrow: paddleA.Paddle = Math.Max(paddleA.Paddle - 1, 0); break;
                    case ConsoleKey.DownArrow: paddleA.Paddle = Math.Min(paddleA.Paddle + 1, Height - paddleA.PaddleSize - 1); break;
                    case ConsoleKey.Escape:
                        System.Console.Clear();
                        System.Console.Write("Pong was closed.");
                        return;
                }
            }
            while (System.Console.KeyAvailable)
            {
                System.Console.ReadKey(true);
            }

            #endregion

        }
        private void UpdateComputerPaddle(PaddleModule paddleB)
        {
            #region Update Computer Paddle

            if (enemyStopwatch.Elapsed > enemyInputDelay)
            {
                if (BallModule.Y < paddleB.Paddle + paddleB.PaddleSize / 2 && BallModule.DY < 0)
                {
                    paddleB.Paddle = Math.Max(paddleB.Paddle - 1, 0);
                }
                else if (BallModule.Y > paddleB.Paddle + paddleB.PaddleSize / 2 && BallModule.DY > 0)
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
            float yIntercept = line.A.Y - line.A.X * slope;
            // find the function's value at parameter "x"
            return x * slope + yIntercept;
        }
    }
}
