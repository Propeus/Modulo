using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Utils.Objetos;

namespace Propeus.Module.Console.CLI
{




    public class MyClass : BaseModule
    {

        public MyClass()
        {

        }

        public MyClass(int param1, string param2, int param3 = 10) : base()
        {
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            MyClass2 = new MyClass2(Param1, Param2, param3);
            // Constructor logic
        }



        public int Param1 { get; }
        public string Param2 { get; }
        public int Param3 { get; }
        public MyClass2 MyClass2 { get; }
    }

    public class MyClass2 : BaseModule
    {
        public MyClass2(int param1, string param2, int param3 = 10) : base()
        {
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            // Constructor logic
        }
        public MyClass2()
        {

        }

        public int Param1 { get; }
        public string Param2 { get; }
        public int Param3 { get; }
    }





    internal class Program
    {
        static void Main(string[] args)
        {


            //var t2 = Propeus.Module.Utils.Objetos.Helper.se(t);
            //var t3 = Propeus.Module.Utils.Objetos.Helper.Desserializar(t2) as MyClass;



            //using (IModuleManager moduleManager = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager())
            //{
            //    using (IModuleManager module = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(moduleManager))
            //    {
            //        var console = module.CreateModule<Propeus.Modulo.Console.Program>();

            //        //System.Console.WriteLine(ObjectSizeCalculator.CalculateObjectSize(module));
            //        //System.Console.WriteLine(ObjectSizeCalculator.CalculateObjectSize(moduleManager));
            //        console.WaitModuleState(State.Off);
            //    }
            //}
        }



    }
}