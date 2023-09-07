using System;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        public class ModuleSemAtributo : IModule
        {
            public bool IsSingleInstance { get; }
            /// <summary>
            /// Version do modelo
            /// </summary>
            public string Version { get; }
            /// <summary>
            /// Representa o estado do objeto.
            /// </summary>
            public State State { get; }
            /// <summary>
            /// Representação amigavel do ojeto. 
            /// <para>
            /// Caso seja nulo o nome da classe herdado será informado na propriedade.
            /// </para>
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Representação alfanumerica e unica do objeto.
            /// </summary>
            public string Id { get; }
            /// <summary>
            /// <see cref="Guid"/> do <see cref="System.Reflection.Assembly" /> atual
            /// </summary>
            public string ManifestId { get; }


            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    disposedValue = true;
                }
            }



            public void Dispose()
            {

                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            public string IdManifesto { get; }

            public void ConfigureModule()
            {
               //Nao faz nada aqui
            }

            public void Launch()
            {
                //Nao faz nada aqui

            }
        }


    }
}