using Propeus.Modulo.IL.Geradores;
using Propeus.Modulo.IL.Proxy;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Propeus.Modulo.IL
{
    /// <summary>
    /// Classe para montagem inicial do Assembly
    /// </summary>
    public partial class ILGerador
    {
        private readonly ILBuilderProxy _proxy;

        /// <summary>
        /// Obtem um clone do <see cref="ILBuilderProxy"/>
        /// </summary>
        internal ILBuilderProxy Proxy => _proxy;

        /// <summary>
        /// Construtor do gerador de IL
        /// </summary>
        /// <param name="nomeAssembly">Nome do assembly</param>
        /// <param name="nomeModulo">Nome do modulo</param>
        public ILGerador(string nomeAssembly = "IL_Gerador_Assembly", string nomeModulo = "IL_Gerador_Modulo")
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(nomeAssembly), AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(nomeModulo);
            _proxy = new ILBuilderProxy();
            Proxy.AdicionarBuilders(assemblyBuilder, moduleBuilder);

            Classes = new List<ILClasse>();
        }

        internal List<ILClasse> Classes { get; }

        /// <summary>
        /// Obtem a instancia da classe
        /// </summary>
        /// <param name="iLClasse"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic ObterInstancia(ILClasse iLClasse, params object[] args)
        {
            if (iLClasse is null)
            {
                throw new ArgumentNullException(nameof(iLClasse));
            }

            return Activator.CreateInstance(iLClasse.TipoGerado, args);
        }

        public static dynamic ClonarClasse<TClasse>(){



        }
    }
}