using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

namespace Propeus.Modulo.IL.Playground
{

    [ModuloContrato(typeof(ModuloTesteA))]
    public interface IModuloContratoA : IModulo
    {
    }

    [ModuloContrato("ModuloTesteB")]
    public interface IModuloContratoB : IModulo
    {
    }

    [ModuloContrato("ModuloTesteC")]
    public interface IModuloContratoC : IModulo
    {
    }

    [Modulo]
    public class ModuloTesteA : ModeloBase, IModuloContratoA
    {
        public ModuloTesteA(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }
    }

    [Modulo]
    public class ModuloTesteB : ModeloBase, IModuloContratoB
    {
        public ModuloTesteB(IGerenciador gerenciador, ModuloTesteA moduloContratoA, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
            if (moduloContratoA is null)
            {
                throw new ArgumentNullException(nameof(moduloContratoA));
            }
        }

        public void CriarInstancia(int a, string txt)
        {
            Console.WriteLine(txt);
            Console.WriteLine(a);
        }
    }

    [Modulo]
    public class ModuloTesteC : ModeloBase
    {
        public ModuloTesteC(IGerenciador gerenciador, ModuloTesteA moduloContratoA, IModuloContratoB moduloContratoB, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
            if (moduloContratoA is null)
            {
                throw new ArgumentNullException(nameof(moduloContratoA));
            }

            if (moduloContratoB is null)
            {
                throw new ArgumentNullException(nameof(moduloContratoB));
            }
        }
    }

    [ModuloContrato("Teste")]
    public interface IModuloCalculadoraContrato : IModulo
    {
        int Calcular(int a, int b);
    }

    [Modulo]
    public class Teste : ModeloBase, IModuloCalculadoraContrato
    {
        public Teste(IGerenciador gerenciador) : base(gerenciador)
        {
        }

        public int Calcular(int a, int b)
        {
            return a + b;
        }
    }


    internal class Program
    {

        private static void Main(string[] args)
        {
            //using (ILGerador iLGerador = new ILGerador())
            //{
            //    var tp = iLGerador.CriarModulo().CriarProxyClasse<Teste>();
            //    tp.Executar();
            //    var t = (Iteste)tp.ObterInstancia();
            //    t.M();
            //    t.M2(10);
            //    t.Whoami();
            //    t.teste2 = 10;

            //}

            //GerenciadorDeTask();
            //return;

            //var modulo = Propeus.Modulo.Core.Gerenciador.Atual.Criar<IModuloContratoB>();
            //Console.WriteLine(modulo.ToString());
            //return;
            var genv2 = new Propeus.Modulo.Dinamico.Gerenciador(Propeus.Modulo.Core.Gerenciador.Atual,new Dinamico.GerenciadorConfiguracao { CarregamentoRapido = false });
            //TODO: Esta dando ruim de cast do mesmo tipo???
            var modulo = genv2.Criar<IModuloCalculadoraContrato>(new object[] { 1, "Ola mundo" });
            Console.WriteLine(modulo.ToString());
            //genv2.ManterVivoAsync().Wait();
            return;

            //using (ILGerador iLGerador = new ILGerador())
            //{

            //    var modulo = iLGerador.CriarModulo("CalculadoraModulo");


            //    //Calculadora(modulo);
            //    //Matematica(modulo);
            //    fDelegate(modulo);

            //}

        }

        private static void GerenciadorDeTask()
        {
            ConcurrentDictionary<int, int> keyValuePairs = new ConcurrentDictionary<int, int>();

            Util.Thread.TaskJob cronTask = new Util.Thread.TaskJob(10);

            Parallel.For(0, 9999, (i) =>
            {
                cronTask.AddTask((state) =>
                {
                    CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)state;
                    if (!keyValuePairs.TryAdd(i, 0))
                    {
                        keyValuePairs[i]++;
                    }

                    if (keyValuePairs[i] >= 3)
                    {
                        cancellationTokenSource.Cancel();
                        keyValuePairs.TryRemove(i, out int _);
                    }
                    //System.Console.WriteLine(i);
                }, TimeSpan.FromSeconds(1), "JOB - " + i);
            });

            while (!cronTask.IsCompleted())
            {
                System.Console.Clear();
                System.Console.WriteLine(cronTask.ToStringRunning());
                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                //cronTask.WaitAll().Wait();
            }
            System.Console.WriteLine(cronTask.ToStringRunning());
            System.Console.WriteLine("FIM");
            return;
        }

        private static void Genv2_OnEvento(object[] args)
        {
            foreach (object o in args)
            {
                global::System.Console.WriteLine(o.ToString());
            }
        }

        private static void fDelegate(ILModulo modulo)
        {

            //Cria uma classe
            ILClasseProvider classe = modulo.CriarClasse("MatematicaBaseDelegate", "Propeus.IL.Exemplo",
                null,
                null,
                new Token[] { Token.Publico });

            //Cria um delegate `int calc(int,int)`
            ILDelegate dlg = classe.CriarDelegate(typeof(int),
                  "calc",
                  new ILParametro[] {
                    new ILParametro("calc", typeof(int)),
                    new ILParametro("calc", typeof(int))
                  });


            //Cria um metodo `int Adicao(int,int)`
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),nome:"p1"),
                new ILParametro("Adicao",typeof(int),nome: "p2")
            });
            _ = mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            modulo.Executar();
            ILMetodo tpMeth = mth;
            _ = classe.NovaVersao();

            //Cria um metodo `void Exibir()`
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(void), "Exibir", new ILParametro[] {
                new ILParametro("Exibir",typeof(int),nome : "p1"),
                new ILParametro("Exibir",typeof(int),nome : "p2")
            });

            //Atribui o metodo `Adicao` dentro de `calc`  `calc a = Adicao`
            _ = mth.AtribuirMetodoEmDelegate(dlg, tpMeth);
            //Chama `a.Invoke(int,int);`
            _ = mth.InvocarDelegate(dlg, mth.Parametros[0], mth.Parametros[1]);
            //Chama `Console.WriteLine(int)`
            _ = mth.ChamarFuncao(typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(int) }));
            // Chama `return;`
            _ = mth.CriarRetorno();

            classe.Executar();
            dynamic cls = classe.ObterInstancia();

            cls.Exibir(1, 1);
        }

        public static Task testeTask()
        {
            System.Console.WriteLine("ok");
            return Task.CompletedTask;
        }

        private static void Matematica(ILModulo modulo)
        {
            ILClasseProvider classe = modulo.CriarClasse("MatematicaBase", "Propeus.IL.Exemplo", null, null, new Token[] { Token.Publico });

            ////Criar metodo de entre valores (teste de multiplos ifs)
            //var mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "EntreValores", new ILParametro[] {
            //    new ILParametro("EntreValores",typeof(int),"menor"),
            //    new ILParametro("EntreValores",typeof(int),"atual"),
            //    new ILParametro("EntreValores",typeof(int),"maior")
            //});
            //mth.Se().MenorOuIgualQue(mth.Parametros[0], mth.Parametros[1]).Ou().MaiorOuIgualQue(mth.Parametros[2], mth.Parametros[1]);
            //mth.CarregarTrue();
            //mth.CriarRetorno();
            //mth.SeFim();
            //mth.CarregarFalse();
            //mth.CriarRetorno();

            //Criar metodo de entre valores (teste de multiplos ifs)
            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(bool), "EntreValores2", new ILParametro[] {
                new ILParametro("EntreValores2",typeof(int),nome : "menor"),
                new ILParametro("EntreValores2",typeof(int),nome : "atual"),
                new ILParametro("EntreValores2",typeof(int),nome : "maior")
            });
            _ = mth.Se(mth.Parametros[0], mth.MenorOuIgualQue, mth.Parametros[1]);
            _ = mth.Ou(mth.Parametros[2], mth.MenorOuIgualQue, mth.Parametros[1]);
            _ = mth.E(mth.Parametros[2], mth.MaiorQue, mth.Parametros[0]);
            _ = mth.CarregarTrue();
            _ = mth.CriarRetorno();
            _ = mth.SeFim();
            _ = mth.CarregarFalse();
            _ = mth.CriarRetorno();

            classe.Executar();

            dynamic calc = classe.ObterInstancia();


        }

        private static void Calculadora(ILModulo modulo)
        {
            ILClasseProvider classe = modulo.CriarClasse("CalculadoraBase", "Propeus.IL.Exemplo", null, null, new Token[] { Token.Publico });

            ILMetodo mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Adicao", new ILParametro[] {
                new ILParametro("Adicao",typeof(int),nome : "p1"),
                new ILParametro("Adicao",typeof(int),nome : "p2")
            });

            _ = mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),nome : "p1"),
                new ILParametro("Subtracao",typeof(int),nome : "p2")
            });

            _ = mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),nome : "p1"),
                new ILParametro("Divisao",typeof(int),nome : "p2")
            });

            _ = mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),nome : "p1"),
                new ILParametro("Multiplicacao",typeof(int),nome : "p2")
            });

            _ = mth.Multiplicar(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();




            classe.Executar();
            dynamic calc = classe.ObterInstancia();

            System.Console.WriteLine("Calculadora");
            System.Console.WriteLine(calc.Adicao(1, 1));
            System.Console.WriteLine(calc.Subtracao(2, 2));
            System.Console.WriteLine(calc.Divisao(3, 3));
            System.Console.WriteLine(calc.Multiplicacao(4, 4));
        }


    }
}