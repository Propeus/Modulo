using System;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.CoreTests
{
    public partial class GerenciadorTests
    {
        [Module]
        public class ModuleSemConstrutor : IModule
        {
            private ModuleSemConstrutor()
            {

            }

            public bool IsSingleInstance { get; }
            public string Version { get; }
            public State State { get; }
            public string Name { get; }
            public string Id { get; }
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
                // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            public string IdManifesto { get; }
        }


    }
}