using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Objects;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data
{
    [Modulo(AutoAtualizavel = false, AutoInicializavel = false)]
    public class RenderModule : ModuloBase
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
