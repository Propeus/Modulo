using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;

namespace Propeus.Module.IL.CoreTests.Modulos
{
    /// <summary>
    /// Um modulo burro, sua funcionalidades não serão utilizados
    /// </summary>
    [Module(Description = "Modulo para testar proxy dinamico", AutoStartable = false, AutoUpdate = false, KeepAlive = false, Singleton = false)]
    public class TesteModule : BaseModule
    {

        public TesteModule()
        {

        }

        public TesteModule(int testecmp, int testeProp, int testeProp2)
        {
            Testecmp = testecmp;
            this.testeProp = testeProp;
            this.testeProp2 = testeProp2;
        }

        public int Testecmp;

        public int this[int valor]
        {
            get => valor;
            set => valor = value;
        }
        public int testeProp { get; }
        public int testeProp2 { get; set; }
        public int testeProp3 { get; }
        public int testeProp4 { get; set; }
        public int TesteMetodo()
        {
            return 0;
        }
        public int TesteMetodo2(int a)
        {
            return a;
        }
        public int TesteMetodo3(int a, int b, int c, int d, int e, int f)
        {
            return a + b + c + d + e + f;
        }

        public int TesteMetodo4(int a = 10, int b = 20)
        {
            return a + b;
        }

        public object TesteMetodo5(object a = null)
        {
            return a;
        }

    }
}