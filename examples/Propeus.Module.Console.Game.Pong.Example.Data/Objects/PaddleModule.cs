using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.Console.Game.Pong.Example.Data.Objects
{
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class PaddleModule : BaseModule
    {
        public event Action<int> ScoreEvent;

        public PaddleModule(int height = 0) : base()
        {
            Score = 0;

            Paddle = height / 3;
            PaddleSize = height / 4;
        }

        public int Paddle { get; set; }
        public int PaddleSize { get; set; }
        public int Score { get; set; }

        public void CalculateColisionBall(BallModule ballModule, bool enemy, int height, int width, float multiplier, float time)
        {


            // Compute Time And New Ball Position
            (float X2, float Y2) = (ballModule.X + time * ballModule.DX, ballModule.Y + time * ballModule.DY);

            // Collisions With Up/Down Walls
            if (Y2 < 0 || Y2 > height)
            {
                ballModule.DY = -ballModule.DY;
                Y2 = ballModule.Y + ballModule.DY;
            }


            if (enemy)
            {
                if (Math.Min(ballModule.X, X2) <= width - 2 && width - 2 <= Math.Max(ballModule.X, X2))
                {
                    int ballPathAtPaddleB = height - (int)GetLineValue(((ballModule.X, height - ballModule.Y), (X2, height - Y2)), width - 2);
                    ballPathAtPaddleB = Math.Max(0, ballPathAtPaddleB);
                    ballPathAtPaddleB = Math.Min(height - 1, ballPathAtPaddleB);
                    if (Paddle <= ballPathAtPaddleB && ballPathAtPaddleB <= Paddle + PaddleSize)
                    {
                        ballModule.DX = -ballModule.DX;
                        ballModule.DX *= multiplier;
                        ballModule.DY *= multiplier;
                        X2 = ballModule.X + time * ballModule.DX;
                    }
                }

                if (X2 < 0)
                {
                    Score++;
                    ScoreEvent?.Invoke(Score);
                }
            }
            else
            {
                if (Math.Min(ballModule.X, X2) <= 2 && 2 <= Math.Max(ballModule.X, X2))
                {
                    int ballPathAtPaddleA = height - (int)GetLineValue(((ballModule.X, height - ballModule.Y), (X2, height - Y2)), 2);
                    ballPathAtPaddleA = Math.Max(0, ballPathAtPaddleA);
                    ballPathAtPaddleA = Math.Min(height - 1, ballPathAtPaddleA);
                    if (Paddle <= ballPathAtPaddleA && ballPathAtPaddleA <= Paddle + PaddleSize)
                    {
                        ballModule.DX = -ballModule.DX;
                        ballModule.DX *= multiplier;
                        ballModule.DY *= multiplier;
                        X2 = ballModule.X + time * ballModule.DX;
                    }
                }

                if (X2 > width)
                {
                    Score++;
                    ScoreEvent?.Invoke(Score);
                }
            }
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
