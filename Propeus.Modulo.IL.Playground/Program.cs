using System;
using System.Threading.Tasks;

using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.IL.Enums;
using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Helpers;

namespace Propeus.Modulo.IL.Playground
{

    [ModuloContrato("ModuloTeste")]
    public interface IModuloContrato : IModulo
    {
        string GetInfo();
    }

    [Modulo]
    public class ModuloTeste : Propeus.Modulo.Core.ModuloBase, IModuloContrato
    {
        public ModuloTeste(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
        {
        }

        public string GetInfo()
        {
            return Gerenciador.ToString();
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

            IGerenciador gen = Propeus.Modulo.Core.Gerenciador.Atual;
            _ = gen.Criar<IModuloContrato>();
            IModuloContrato modulo = gen.Obter<ModuloTeste>();
            System.Console.WriteLine(gen);
            System.Console.WriteLine(modulo);
            System.Console.WriteLine(modulo.GetInfo());

            //using (ILGerador iLGerador = new ILGerador())
            //{

            //    var modulo = iLGerador.CriarModulo("CalculadoraModulo");


            //    //Calculadora(modulo);
            //    //Matematica(modulo);
            //    fDelegate(modulo);

            //}

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
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });
            _ = mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            modulo.Executar();
            ILMetodo tpMeth = mth;
            _ = classe.NovaVersao();

            //Cria um metodo `void Exibir()`
            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(void), "Exibir", new ILParametro[] {
                new ILParametro("Exibir",typeof(int),"p1"),
                new ILParametro("Exibir",typeof(int),"p2")
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
                new ILParametro("EntreValores2",typeof(int),"menor"),
                new ILParametro("EntreValores2",typeof(int),"atual"),
                new ILParametro("EntreValores2",typeof(int),"maior")
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
                new ILParametro("Adicao",typeof(int),"p1"),
                new ILParametro("Adicao",typeof(int),"p2")
            });

            _ = mth.Soma(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Subtracao", new ILParametro[] {
                new ILParametro("Subtracao",typeof(int),"p1"),
                new ILParametro("Subtracao",typeof(int),"p2")
            });

            _ = mth.Subitrair(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Divisao", new ILParametro[] {
                new ILParametro("Divisao",typeof(int),"p1"),
                new ILParametro("Divisao",typeof(int),"p2")
            });

            _ = mth.Dividir(mth.Parametros[0], mth.Parametros[1]);
            _ = mth.CriarRetorno();

            mth = classe.CriarMetodo(new Token[] { Token.Publico, Token.OcutarAssinatura }, typeof(int), "Multiplicacao", new ILParametro[] {
                new ILParametro("Multiplicacao",typeof(int),"p1"),
                new ILParametro("Multiplicacao",typeof(int),"p2")
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