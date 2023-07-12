using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Modulos;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Scene;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Objects
{
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class PaddleModule : BaseModule
    {
        public PaddleModule(int height = 0) : base(false)
        {
            Score = 0;
        }

        public void CriarInstancia(int height = 0)
        {
            Paddle = height / 3;
            PaddleSize = height / 4;
        }

        public int Paddle { get; set; }
        public int PaddleSize { get; set; }
        public int Score { get; set; }
    }
}
