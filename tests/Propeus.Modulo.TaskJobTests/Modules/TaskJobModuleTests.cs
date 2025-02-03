using Propeus.Module.Taskjob.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Propeus.Module.Abstract.Interfaces;
using Propeus.Module.Taskjob.Contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Module.Taskjob.Modules.Tests
{
    [TestClass()]
    public class TaskJobModuleTests
    {
        private IModuleManager gerenciador;
        private ITaskJobContract module;

        //[TestInitialize] Nao usse isto, pois alguns assemblies podem nao ser carregados de imediato pelo dotnet
        public void Begin()
        {
            gerenciador = Manager.Dynamic.ModuleManagerExtensions.CreateModuleManager(Manager.ModuleManagerExtensions.CreateModuleManager());
        }

        [TestCleanup]
        public void End()
        {
            gerenciador.Dispose();
        }

        [TestMethod()]
        public void TaskJobModuleTest()
        {
            Begin();
            module = gerenciador.CreateModule<ITaskJobContract>();
        }

        [TestMethod()]
        public void RegisterJobTest()
        {

            TaskJobModuleTest();
            var bag = new TaskJobModule.BagAction();
            bag.Add("teste_bag", "teste bag");
            module.RegisterJob((bags) =>
            {
                Assert.IsNotNull(bags);
                Assert.IsNotNull(bags.GetBag("teste_bag"));
                Console.WriteLine(bags.GetBag("teste_bag"));
            }, bag);

            var bag2 = new TaskJobModule.BagAction();
            bag2.Add("teste_bag_nomeado", "teste bag nomeado");
            module.RegisterJob((bags) =>
            {
                Assert.IsNotNull(bags);
                Assert.IsNotNull(bags.GetBag("teste_bag_nomeado"));
                Console.WriteLine(bags.GetBag("teste_bag_nomeado"));
            }, bag2, "job nomeado");


            module.RegisterJob((args) => { Console.WriteLine(nameof(RegisterJobTest)); });
        }

        [TestMethod()]
        public void RegisterRecurringJobTest()
        {
            TaskJobModuleTest();
            var bag = new TaskJobModule.BagAction();
            bag.Add("teste_bag", "teste bag");
            module.RegisterRecurringJob((bags) =>
            {
                Assert.IsNotNull(bags);
                Assert.IsNotNull(bags.GetBag("teste_bag"));
                Console.WriteLine(bags.GetBag("teste_bag"));

            }, TimeSpan.FromSeconds(1), bag);

            var bag2 = new TaskJobModule.BagAction();
            bag2.Add("teste_bag_nomeado", "teste bag nomeado");
            module.RegisterRecurringJob((bags) =>
            {
                Assert.IsNotNull(bags);
                Assert.IsNotNull(bags.GetBag("teste_bag_nomeado"));
                Console.WriteLine(bags.GetBag("teste_bag_nomeado"));
            }, TimeSpan.FromSeconds(1), bag2, "job nomeado");

            module.RegisterRecurringJob((bags) =>
            {
                Assert.IsNotNull(bags);
                Console.WriteLine(module.ToStringView());
            }, TimeSpan.FromSeconds(1));

            module.WaitAll();
        }

        [TestMethod()]
        public void UnregisterJobTest()
        {

            TaskJobModuleTest();

            var bag = new TaskJobModule.BagAction();
            bag.Add("teste_bag_nomeado", "teste bag nomeado");
            module.RegisterJob((bags) => { Console.WriteLine(bags.GetBag("teste_bag_nomeado")); }, bag, "job nomeado");

            module.UnregisterJob("job nomeado");
        }

        [TestMethod()]
        public void WaitAllTest()
        {

            TaskJobModuleTest();

            module.RegisterJob((bags) => { Task.Delay(TimeSpan.FromSeconds(20)).Wait(); }, null, "job nomeado");
            module.WaitAll(TimeSpan.FromSeconds(10));
            UnregisterJobTest();
        }

        [TestMethod()]
        public void ToStringTest()
        {

            TaskJobModuleTest();
            module.RegisterJob((args) => { Console.WriteLine(module.ToString()); });

            Assert.IsNotNull(module.ToString());
            module.WaitAll();
        }

        [TestMethod()]
        public void ToStringViewTest()
        {

            TaskJobModuleTest();
            module.RegisterJob((args) => { Console.WriteLine(module.ToStringView()); });
            Assert.IsNotNull(module.ToStringView());
            module.WaitAll();
        }








    }
}