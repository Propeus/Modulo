﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Propeus.Modulo.Abstrato;
using Propeus.Modulo.Abstrato.Interfaces;
using Propeus.Modulo.Util;

namespace Propeus.Modulo.Dinamico
{
    /// <summary>
    /// Modelo para obter informações do binario do modulo
    /// </summary>
    public class ModuloBinario : BaseModelo, IModuloBinario
    {

        public ModuloBinario(string caminho)
        {
            Caminho = caminho;
            var assem = AppDomain.CurrentDomain.GetAssemblies().Select(asm => asm.Location).ToArray();
            //Nao pode carregaar em memoria um modulo que esta em execucao
            if (!assem.Contains(caminho))
            {
                if (File.Exists(caminho))
                {
                    Memoria = new MemoryStream();
                    using (FileStream arquivo = new(caminho, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader binario = new(arquivo))
                        {
                            binario.BaseStream.CopyTo(Memoria);
                            binario.Close();
                        }
                        arquivo.Close();
                    }
                    _ = Memoria.Seek(0, SeekOrigin.Begin);
                    Hash = Memoria.GetBuffer().Hash();
                    ModuloInformacao = new ModuloInformacao(this);
                }
            }

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
        /// ModuloInformacao mapeados do binario
        /// </summary>
        public IModuloInformacao ModuloInformacao { get; }

        /// <summary>
        /// "Ponteiro" onde se encontra o binario em memoria
        /// </summary>
        public Span<byte> Referencia => Memoria.GetBuffer().AsSpan();
        /// <summary>
        /// Verifica se o binaro possui algum modulo valido
        /// </summary>
        public bool BinarioValido => ModuloInformacao?.ModulosDescobertos > 0;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            _ = sb.Append("Binario valido: ").Append(BinarioValido).AppendLine();
            _ = sb.Append(ModuloInformacao).AppendLine();

            return sb.ToString();

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ModuloInformacao?.Dispose();

        }

    }
}
