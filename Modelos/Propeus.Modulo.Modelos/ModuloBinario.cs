using Propeus.Modulo.Modelos.Interfaces;
using Propeus.Modulo.Util.Objetos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Propeus.Modulo.Modelos
{

    /// <summary>
    /// Modelo para obter informações do binario do modulo
    /// </summary>
    public class ModuloBinario : BaseModelo, IModuloBinario
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caminho">Caminho fisico do modulo (dll)</param>
        public ModuloBinario(string caminho)
        {
            Caminho = caminho;
            Memoria = new MemoryStream();
            using (FileStream arquivo = new FileStream(caminho, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binario = new BinaryReader(arquivo))
                {
                    binario.BaseStream.CopyTo(Memoria);
                    binario.Close();
                }
                arquivo.Close();
            }
            Memoria.Seek(0, SeekOrigin.Begin);
            Hash = Memoria.GetBuffer().Hash();
            Modulos = new Collection<IModuloInformacao>();
        }

        /// <summary>
        /// Caminho onde se encontra o binario do modulo
        /// </summary>
        public string Caminho { get; }

        /// <summary>
        /// Conteudo do modulo armazenado em memoria
        /// </summary>
        public MemoryStream Memoria { get; }

        /// <summary>
        /// Hash do binario do modulo
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// Modulos mapeados do binario
        /// </summary>
        public ICollection<IModuloInformacao> Modulos { get; }

        /// <summary>
        /// "Ponteiro" onde se encontra o binario em memoria
        /// </summary>
        public Span<byte> Referencia => Memoria.GetBuffer().AsSpan();

        /// <summary>
        /// Registra novas informações de modulo.
        /// </summary>
        /// <param name="moduloInformarcao"></param>
        public void Registrar(IModuloInformacao moduloInformarcao)
        {
            Modulos.Add(moduloInformarcao);
        }

        /// <summary>
        /// Remove as informações de modulo.
        /// </summary>
        /// <param name="moduloInformarcao"></param>
        public void Remover(IModuloInformacao moduloInformarcao)
        {
            Modulos.Remove(moduloInformarcao);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            foreach (IModuloInformacao item in Modulos)
            {
                if (!item.Disposed)
                {
                    item.Dispose();
                }
            }

        }

    }
}
