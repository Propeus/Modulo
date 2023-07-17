using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

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


        public BallModule()
        {
            random = new Random();

        }



        public void CriarInstancia(float x = 0, float y = 0)
        {
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

    }
}
