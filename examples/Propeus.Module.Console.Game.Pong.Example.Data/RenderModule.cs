using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Console.Game.Pong.Example.Data.Objects;

namespace Propeus.Module.Console.Game.Pong.Example.Data
{
    [Module(AutoUpdate = false, AutoStartable = false, Singleton = true)]
    public class RenderModule : BaseModule
    {
        public RenderModule() : base()
        {

        }

        public void RenderPaddles(PaddleModule paddleA, PaddleModule paddleB, int height, int width)
        {
            #region Render Paddles

            for (int i = 0; i < height; i++)
            {
                System.Console.SetCursorPosition(2, i);
                System.Console.Write(paddleA.Paddle <= i && i <= paddleA.Paddle + paddleA.PaddleSize ? '█' : ' ');
                System.Console.SetCursorPosition(width - 2, i);
                System.Console.Write(paddleB.Paddle <= i && i <= paddleB.Paddle + paddleB.PaddleSize ? '█' : ' ');
            }

            #endregion
        }

        public void RenderBalls(BallModule ballModule, bool clearBall)
        {


            System.Console.SetCursorPosition((int)ballModule.X, (int)ballModule.Y);

            if (clearBall)
            {
                System.Console.Write(ballModule.BallCharacterEraseIcon);
            }
            else
            {
                System.Console.Write(ballModule.BallCharacterIcon);
            }
        }

        public void Clear()
        {
            System.Console.Clear();
        }

    }
}
