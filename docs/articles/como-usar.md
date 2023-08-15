# Como usar
Esta pagina irá te mostrar como usar o gerenciador de modulos em diversas situações, desde console até sistema web MVC (Razor)


## Metodo

# [Dinamico](#tab/d)
Os tutoriais abaixo irão utilziar o Gerenciador Dinamico
# [Não dinamico](#tab/nd)
Os tutoriais abaixo irão utilziar o Gerenciador Core

---

## Inicialização

# [Console Aplication](#tab/ca/d)
Para usar o gerenciador dinamico
```cs
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Example
{
    internal class Program
    {
        private static void Main()
        {
            ExemploPropeusModuloCore();
            System.Console.ResetColor();
        }

        private static void ExemploPropeusModuloCore()
        {
           /**
            * Este gerenciador é um **modulo** porém se comporta como um gerenciador
            * 
            * Para inicializar ele é necessário passar um gerenciador "nativo" ou de nível superior
            * **/
           using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                //Adicione seu codigo aqui...
            }

            //Caso deseje um exemplo mais completo, acesse o projeto Propeus.Modulo.Console.Example
        }
    }
}
```
# [Jogos](#tab/jgs/d)
> [!NOTE]
> Para o exemplo abaixo utiliza o projeto pong da [Microsoft](https://github.com/dotnet/dotnet-console-games/blob/main/Projects/Pong) porém, foi modificado para o contexto de modulos

Para inicializar, crie um projeto console
```cs
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Game.Pong.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using (IModuleManager gen = Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                //Gerencie o seu jogo aqui...
            }
        }
    }
}
```



O seu projeto principal deve
# [Worker Service](#tab/ws/d)
> [!TIP]
> Os exemplos abaixo inicializam automaticamente os modulos de worker service
```cs
using Propeus.Modulo.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseGerenciador(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager()))
    .Build();

host.Run();
```

ou 

```cs
using Propeus.Modulo.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager()));
    })
    .Build();

host.Run();
```

ou metodo padrão de inicializar o worker service no .NET

```cs
using Propeus.Modulo.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
      
        services.AddHostedService<Propeus.Modulo.WorkerService.Exmaple.DLL.Worker>();
    })
    .Build();

host.Run();
```
> [!IMPORTANT]
> Este metodo demonstra que é possivel inicializar um modulo sem qualquer gerenciador, como se o modulo fosse somente um worker service qualquer


# [MVC Razor](#tab/mvc/d)
d

# [Console Aplication](#tab/ca/nd)
Para usar o gerenciador core
```cs
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Example
{
    internal class Program
    {
        private static void Main()
        {
            ExemploPropeusModuloCore();
            System.Console.ResetColor();
        }

        private static void ExemploPropeusModuloCore()
        {
            /**
             * Este gerenciador basicamente é uma DI em formato de modulo
             * Uma vez que termina o escopo, todos os modulos criados dentro dele são eliminados assim como o gerenciador, 
             * más ao chamar a propriedade Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager() Uma nova instancia será criada
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                //Adicione seu codigo aqui...
            }

            //Caso deseje um exemplo mais completo, acesse o projeto Propeus.Modulo.Console.Example
        }
    }
}
```
# [Jogos](#tab/jgs/nd)
b
# [Worker Service](#tab/ws/nd)
c
# [MVC Razor](#tab/mvc/nd)
d

---

## Como usar

# [Console Aplication](#tab/ca/d)
Esse tutorial é para demonstrar como usa o gerenciador dinamico. Para iniciar o tutorial adicione o modulo abaixo no seu projeto e sua interface de contrato

```cs
using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Example
{
    
    /**
     * Para criar um modulo, é necessário ter o atributo ModuleProxy e herdar de IModule, entretanto é recomendável usar a classe BaseModule, 
     * pois ja foi implementado os recursos necessários para o modulo funcionar
     * 
     * Observação, durante o uso do gerenciador dinâmico, lembre-se sempre de deixar o contrato como **public** pois por ser dinâmico haverá erro de acessibilidade 
     * durante a criação do modulo
     * **/
    [Module]
    public class ModuloDeExemploParaPropeusModuloDinamico : BaseModule
    {
        //O parâmetro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilizando uma unica instancia quando houver.
        //Por padrão o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloDinamico() : base(false)
        {
        }

        //Implemente os metodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Ola mundo!");
            System.Console.WriteLine("Este é um modulo dinamico em funcionamento!");
        }

        public void EscreverOutraCoisaParaOutroContrato()
        {
            System.Console.WriteLine("Esta função está em outro contrato !!!");
        }

        public void EscreverOutraCoisaQueOContratoNaoPossui()
        {
            System.Console.WriteLine("Esta função não está no contrato ;)");
        }

        /**Obs.:
         * Qualquer modulo, possui a liberdade de manipular o gerenciador, sendo assim, podendo até mesmo remover outros módulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IModuleManager como parâmetro, entretendo, será necessário criar um novo nível de gerenciador 
         * que consiga realizar esta operação
        **/
    }

    /**
     * Para criar um contrato, é necessário ter o atributo ModuloContrato e herdar de IModule
     * O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Modulo.Core, recomendo utilizar o tipo
     * 
     * Observação, durante o uso do gerenciador dinâmico, lembre-se sempre de deixar o contrato como **public** pois por ser dinâmico haverá erro de acessibilidade 
     * durante a criação do modulo
     * **/
    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamico : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
    
   [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloDinamico))]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodo : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }

    /**
     * Para o caso de carregamento dinâmico, é necessário o uso do nome do modulo no atributo ModuloContrato, já que se considera que 
     * o projeto atual nao possui qualquer referencia com o projeto que implementa o modulo
     * **/
    [ModuleContract("ModuloDeExemploParaPropeusModuloDinamico")]
    public interface IInterfaceDeContratoDeExemploParaPropeusModuloDinamicoComOutroMetodoSemUsoDeTypeOf : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOutraCoisaParaOutroContrato();
    }
}
```

Com o modulo e contratos criados, vá para o metodo Main() do seu projeto console e adicione este código abaixo
```cs
System.Console.ForegroundColor = System.ConsoleColor.Yellow;
System.Console.WriteLine("Exemplo de uso do Propeus.ModuleProxy.Dinamico");

/**
    * Este gerenciador é um **modulo** porém se comporta como um gerenciador
    * 
    * Para inicializar ele é necessário passar um gerenciador "nativo" ou de nível superior
    * **/
using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
{
    //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
    IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
    modulo.EscreverOlaMundo();
    //Veja em seu console o funcionamento do modulo
}
```

O resultado final deve ser isso aqui:
```cs
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Example
{
    internal class Program
    {
        private static void Main()
        {
          System.Console.ForegroundColor = System.ConsoleColor.Yellow;
          System.Console.WriteLine("Exemplo de uso do Propeus.ModuleProxy.Dinamico");

            /**
             * Este gerenciador é um **modulo** porém se comporta como um gerenciador
             * 
             * Para inicializar ele é necessário passar um gerenciador "nativo" ou de nível superior
             * **/
            using (IModuleManager gerenciador = Propeus.Modulo.Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                //Note que o modulo ModuloDeExemploParaPropeusModuloDinamico **não** implementa a interface de contrato, normalmente haveria um erro de cast por parte do programa
                IInterfaceDeContratoDeExemploParaPropeusModuloDinamico modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloDinamico>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            //Caso deseje um exemplo mais completo, acesse o projeto Propeus.Modulo.Console.Example
        }
    }
}
```

A saida em seu console deve ser: 
```txt
Ola mundo!
Este é um modulo dinamico em funcionamento!
```

> [!TIP]
> Observe que o modulo **não possui a interface de contrato implementado**, isso mostra que o gerenciador dinamico funciona para o seu proposito.
# [Jogos](#tab/jgs/d)
> [!NOTE]
> Para o exemplo abaixo utiliza o projeto pong da [Microsoft](https://github.com/dotnet/dotnet-console-games/blob/main/Projects/Pong) porém, foi modificado para o contexto de modulos

Para inicializar o jogo, crie uma interface de contrato para chamar a cena principal
```cs
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Game.Pong.Example
{
    [ModuleContract("WindowModule")]
    public interface IWindowModuleContract : IModule
    {
        Task Main();
    }
}
```

Depois crie a chamada dentro do `using` do seu programa principal
```cs
    if (!gen.ExistsModule(typeof(IWindowModuleContract)))
    {
        await gen.CreateModule<IWindowModuleContract>().Main();
    }
    else
    {
        await gen.GetModule<IWindowModuleContract>().Main();
    }
```

O resultado deverá ser semelhante a isso:
```cs
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Game.Pong.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using (IModuleManager gen = Dinamico.ModuleManagerExtensions.CreateModuleManager(ModuleManagerExtensions.CreateModuleManager()))
            {
                if (!gen.ExistsModule(typeof(IWindowModuleContract)))
                {
                    await gen.CreateModule<IWindowModuleContract>().Main();
                }
                else
                {
                    await gen.GetModule<IWindowModuleContract>().Main();
                }
            }
        }
    }
}
```

> [!IMPORTANT]
> Antes de construir o programa principal do jogo, devemos construir os modulos necessários

Crie um modulo para controlar a bola
```cs
using System.Diagnostics;

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

        public Stopwatch StopWatch { get; }

        public BallModule()
        {
            random = new Random();
            StopWatch = Stopwatch.StartNew();
            BallCharacterIcon = 'O';
            BallCharacterEraseIcon = ' ';
        }

        public char BallCharacterIcon { get; set; }
        public char BallCharacterEraseIcon { get; set; }

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

        public void UpdateBallNewPosition(float time)
        {
            X += time * DX;
            Y += time * DY;
        }

    }
}
```

Depois crie o modulo para controlar as paletas
```cs
using System.Diagnostics;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Objects
{
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class PaddleModule : BaseModule
    {
        public event Action<int> ScoreEvent;

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

        public void CalculateColisionBall(BallModule ballModule, bool enemy, int height, int width, float multiplier,float time)
        {


            // Compute Time And New Ball Position
            (float X2, float Y2) = (ballModule.X + (time * ballModule.DX), ballModule.Y + (time * ballModule.DY));

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
                        X2 = ballModule.X + (time * ballModule.DX);
                    }
                }

                if (X2 < 0)
                {
                    Score++;
                    this.ScoreEvent?.Invoke(Score);
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
                        X2 = ballModule.X + (time * ballModule.DX);
                    }
                }

                if (X2 > width)
                {
                    Score++;
                    this.ScoreEvent?.Invoke(Score);
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
            float yIntercept = line.A.Y - (line.A.X * slope);
            // find the function's value at parameter "x"
            return (x * slope) + yIntercept;
        }
    }
}

```

Crie um modulo para gerenciar o controle, que no caso será o teclado
```cs
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
    [Module(AutoUpdate = false, AutoStartable = false)]
    public class ControlsModule : BaseModule
    {
        private CancellationTokenSource _cancelationTokenSouce;
        Task _controlPlayer;
        bool _exit;

        public event Action Up;
        public event Action Down;
        public event Action Exit;

        public ControlsModule() : base(true)
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
                    if(_controlPlayer.Status == TaskStatus.Running)
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

```

Por fim, crie o modulo que irá renderizar os objetos na tela do console
```cs
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

```

> [!TIP]
> Obviamente, por ser um jogo simples, não há necessidade de criar um modulo para renderizar os objetos na tela, porém, como este tutorial é para exemplificar o uso de modulos em
um contexto de jogos, foi feito desta forma.

Agora com todos os objetos criados, crie o modulo principal que irá juntar todos os objetos necessarios para o jogo funcionar e o renderizador para exibir em tela:
```cs
using System.Diagnostics;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Console.Game.Pong.Example.Data.Objects;

namespace Propeus.Modulo.Console.Game.Pong.Example.Data.Scene
{
    [Module(AutoUpdate = true, AutoStartable = false)]
    public class WindowModule : BaseModule
    {
        private readonly IModuleManagerArguments gerenciador;
        private readonly float multiplier;



        public WindowModule(IModuleManager gerenciador, int? width, int? height, ControlsModule controlsModule) : base(true)
        {
            this.gerenciador = gerenciador as IModuleManagerArguments;
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
            flg_loop=false;
        }

        private void PaddleA_ScoreEvent(int obj)
        {
            flg_loop=false;

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
```


Agora execute o jogo :)

> [!TIP]
> Este exemplo está na pasta `examples` com o nome `Propeus.Modulo.Console.Game.Pong.Example` e `Propeus.Modulo.Console.Game.Pong.Example.Data`

# [Worker Service](#tab/ws/d)
c
# [MVC Razor](#tab/mvc/d)
d

# [Console Aplication](#tab/ca/nd)
Esse tutorial é para demonstrar como usa o gerenciador core. Para iniciar o tutorial adicione o modulo abaixo no seu projeto e sua interface de contrato

```cs
using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Attributes;

namespace Propeus.Modulo.Console.Example
{
    
    //Para criar um modulo, é necessário ter o atributo ModuleProxy e herdar de IModule, entretanto é recomendável usar a classe BaseModule, pois ja foi implementado os recursos necessários para o modulo funcionar
    //Deve ser implementado a interface de contrato para que o Propeus.Modulo.Core funcione corretamente
    [Module]
    internal class ModuloDeExemploParaPropeusModuloCore : BaseModule, IInterfaceDeContratoDeExemploParaPropeusModuloCore
    {
        //O parâmetro instanciaUnica indica se o gerenciador pode criar uma nova instancia sempre que for solicitado ou se deve ser utilizando uma unica instancia quando houver.
        //Por padrão o valor para instanciaUnica é false
        public ModuloDeExemploParaPropeusModuloCore() : base(false)
        {
        }

        //Implemente os métodos e propriedades que o modulo deve possuir.
        public void EscreverOlaMundo()
        {
            System.Console.WriteLine("Ola mundo!");
            System.Console.WriteLine("Este é um modulo em funcionamento!");
        }

        /**Obs.:
         * Qualquer modulo, possui a liberdade de manipular o gerenciador, sendo assim, podendo até mesmo remover outros módulos ou até este mesmo.
         * Este projeto da a liberdade de criar um novo modulo que nao precise de um IModuleManager como parâmetro, entretendo, será necessário criar um novo nível de gerenciador 
         * que consiga realizar esta operação
        **/
    }

    //Para criar um contrato, é necessário ter o atributo ModuloContrato e herdar de IModule
    //O atributo ModuloContrato aceita tanto tipo como o nome dele, más para o cao do Propeus.Modulo.Core, recomendo utilizar o tipo
    [ModuleContract(typeof(ModuloDeExemploParaPropeusModuloCore))]
    internal interface IInterfaceDeContratoDeExemploParaPropeusModuloCore : IModule
    {
        //Crie os métodos e propriedades que o modulo deve possuir.
        //Não é necessário escrever todas as funcionalidades do modulo no contrato, somente aquilo que será utilizado.
        void EscreverOlaMundo();
    }
}
```

Com o modulo e contrato criados, vá para o metodo Main() do seu projeto console e adicione este código abaixo
```cs
System.Console.ForegroundColor = System.ConsoleColor.Green;
System.Console.WriteLine("Exemplo de uso do Propeus.Modulo.Core");

/**
    * Este gerenciador basicamente é uma DI em formato de modulo
    * Uma vez que termina o escopo, todos os modulos criados dentro dele são eliminados assim como o gerenciador, 
    * más ao chamar a propriedade Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager() Uma nova instancia será criada
    * **/
using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
{
    IInterfaceDeContratoDeExemploParaPropeusModuloCore modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloCore>();
    modulo.EscreverOlaMundo();
    //Veja em seu console o funcionamento do modulo
}
```

O resultado final deve ser isso aqui:
```cs
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Core;

namespace Propeus.Modulo.Console.Example
{
    internal class Program
    {
        private static void Main()
        {
           /**
             * Este gerenciador basicamente é uma DI em formato de modulo
             * Uma vez que termina o escopo, todos os modulos criados dentro dele são eliminados assim como o gerenciador, 
             * más ao chamar a propriedade Propeus.Modulo.Core.ModuleManagerExtensions.CreateModuleManager() Uma nova instancia será criada
             * **/
            using (IModuleManager gerenciador = ModuleManagerExtensions.CreateModuleManager())
            {
                IInterfaceDeContratoDeExemploParaPropeusModuloCore modulo = gerenciador.CreateModule<IInterfaceDeContratoDeExemploParaPropeusModuloCore>();
                modulo.EscreverOlaMundo();
                //Veja em seu console o funcionamento do modulo
            }

            //Caso deseje um exemplo mais completo, acesse o projeto Propeus.Modulo.Console.Example
        }
    }
}
```

A saida em seu console deve ser: 
```txt
Ola mundo!
Este é um modulo em funcionamento!
```

# [Jogos](#tab/jgs/nd)
b
# [Worker Service](#tab/ws/nd)
c
# [MVC Razor](#tab/mvc/nd)
d

---