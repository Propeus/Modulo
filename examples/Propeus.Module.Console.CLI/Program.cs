using System.Reflection;
using System.Runtime.InteropServices;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Interfaces;
using Propeus.Modulo.Console;

namespace Propeus.Module.Console.CLI
{
    public class MyClass
    {
        public MyClass(int param1, string param2, int param3 = 10)
        {
            // Constructor logic
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ParameterInfo[] constructorParams = typeof(MyClass).GetConstructors()[0].GetParameters();
            object[] userParams = Array.Empty<object>();


            try
            {
                object[] result = Propeus.Module.Utils.Objetos.Helper.JoinParameterValue(constructorParams, userParams);
                System.Console.WriteLine(string.Join(", ", result));
            }
            catch (ArgumentException ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }

          

            //using (IModuleManager moduleManager = Propeus.Module.Manager.ModuleManagerExtensions.CreateModuleManager())
            //{
            //    using (IModuleManager module = Propeus.Module.Manager.Dinamic.ModuleManagerExtensions.CreateModuleManager(moduleManager))
            //    {
            //        var console = module.CreateModule<ConsoleModulo>();

            //        //System.Console.WriteLine(ObjectSizeCalculator.CalculateObjectSize(module));
            //        //System.Console.WriteLine(ObjectSizeCalculator.CalculateObjectSize(moduleManager));
            //        console.WaitModuleState(State.Off);
            //    }
            //}
        }

      

    }
}