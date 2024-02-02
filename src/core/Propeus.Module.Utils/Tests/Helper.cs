using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeus.Module.Utils.Tests
{
    /// <summary>
    /// Classe de ajuda para realizar testes em DEBUG
    /// </summary>
    public static partial class Helper
    {
        private class ST_Debug : IDisposable
        {
            private Stopwatch _stopwatch = new Stopwatch();

            public ST_Debug(string? nameTest, string? descriptionTest)
            {
                _dateStartTest = DateTime.Now;
                _stopwatch.Start();
                this._nameTest = nameTest;
                this._descriptionTest = descriptionTest;
            }

            #region Dispose
            private bool disposedValue;
            private readonly DateTime _dateStartTest;
            private readonly string? _nameTest;
            private readonly string? _descriptionTest;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        _stopwatch.Stop();
                        Debug.WriteLine($"Name: {_nameTest} | description: {_descriptionTest} | date & time start: {_dateStartTest} | duration: {_stopwatch.Elapsed}");
                    }

                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }

        /// <summary>
        /// Inicia um contador para realizar testes
        /// </summary>
        /// <param name="nameTest"></param>
        /// <param name="descriptionTest"></param>
        /// <returns></returns>
        public static IDisposable StartTest(string? nameTest, string? descriptionTest)
        {
            return new ST_Debug(nameTest, descriptionTest);
        }
    }
}
