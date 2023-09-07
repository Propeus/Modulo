using System.Diagnostics;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Objects
{
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class BallModule : BaseModule
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float DX { get; set; }
        public float DY { get; set; }

        private readonly Random random;

        public Stopwatch StopWatch { get; }

        public BallModule(float x = 0, float y = 0)
        {
            random = new Random();
            StopWatch = Stopwatch.StartNew();
            BallCharacterIcon = 'O';
            BallCharacterEraseIcon = ' ';

            float randomFloat = (float)random.NextDouble() * 2f;
            DX = Math.Max(randomFloat, 1f - randomFloat);
            DY = 1f - DX;
            X = x / 2f;
            Y = y / 2f;
            if (random.Next(2) == 0)
            {
                DX = -DX;
            }
            if (random.Next(2) == 0)
            {
                DY = -DY;
            }
        }

        public char BallCharacterIcon { get; set; }
        public char BallCharacterEraseIcon { get; set; }

        public void UpdateBallNewPosition(float time)
        {
            X += time * DX;
            Y += time * DY;
        }

    }
}
