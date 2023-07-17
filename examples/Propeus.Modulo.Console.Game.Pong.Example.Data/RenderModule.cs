using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Objects;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data
{
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class RenderModule : BaseModule
    {
        public RenderModule() : base(true)
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

    }
}
