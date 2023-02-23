using System;
using System.Text;

using Propeus.Modulo.Abstrato.Interfaces;

namespace Propeus.Modulo.Abstrato
{
    /// <summary>
    /// Classe com o modelo base para todo o projeto
    /// </summary>
    public class BaseModelo : IBaseModelo
    {

        /// <summary>
        /// Inicia um modelo basico
        /// </summary>
        public BaseModelo()
        {
            Nome = GetType().Name;
            Estado = Estado.Inicializado;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Inicia um modelo com um nome customizado
        /// </summary>
        /// <param name="nome">Nome do modelo</param>
        public BaseModelo(string nome)
        {
            Nome = nome;
            Estado = Estado.Inicializado;
            Id = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// Versão do assembly onde o modulo foi carregado. {Maj}.{Min}.{Build}
        /// </summary>
        public virtual string Versao
        {
            get
            {
                Version ver = GetType().Assembly.GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }
  

        /// <summary>
        /// Representa o estado do objeto. É definido inicialmente como 1 (iniciado) ou 0 (disposed), podendo ser alterado pelo usuario final ou pela regra de negocio
        /// </summary>
        public Estado Estado { get; set; }

        /// <summary>
        /// Representação amigavel do objeto. Caso seja nulo o nome da classe herdado será informado na propriedade.
        /// </summary>
        public string Nome { get; protected set; }

        /// <summary>
        /// Representação alfanumerica e unica do objeto.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Exibe informações basicas sobre o modelo
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();

            _ = sb.Append("Nome: ").Append(Nome).AppendLine();
            _ = sb.Append("Estado: ").Append(Estado).AppendLine();
            _ = sb.Append("Id: ").Append(Id).AppendLine();
            _ = sb.Append("Versao: ").Append(Versao).AppendLine();

            return sb.ToString();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Estado = Estado.Desligado;
                }

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        //~BaseModelo()
        //{
        //    // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //    Dispose(false);
        //}

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            //GC.SuppressFinalize(this);
        }
        #endregion

    }
}
