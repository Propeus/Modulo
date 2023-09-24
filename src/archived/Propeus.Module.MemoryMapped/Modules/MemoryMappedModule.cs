using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Propeus.Module.Abstract;
using Propeus.Module.Abstract.Attributes;
using Propeus.Module.Abstract.Interfaces;

namespace Propeus.Modulo.MemoryMapped.Modules
{
    /// <summary>
    /// Modulo para compartilhar outros modulos entre processos
    /// </summary>
    [Module(AutoStartable = false, AutoUpdate = false, Description = "Modulo para compartilhar outros modulos entre processos", KeepAlive = false, Singleton = true)]
    public class MemoryMappedModule : BaseModule
    {
        private MemoryMappedFile _sharedIndex;
        private MemoryMappedViewAccessor _acc;

        /// <summary>
        /// 
        /// </summary>
        public MemoryMappedModule(IModuleManager moduleManager)
        {
            //Criar um sistema de compatilhamento de modulo por memoria
        }

        public override void ConfigureModule()
        {
            _sharedIndex = MemoryMappedFile.CreateOrOpen("module_shared_index", 1024, MemoryMappedFileAccess.ReadWriteExecute);
            Stream stream = _sharedIndex.CreateViewStream(0,1024);
            // Cria um ponteiro para a estrutura MyObject
            IntPtr pointer = IntPtr.Add((int)stream.Position, sizeof(long));
            // Cria uma instância do objeto .NET na memória mapeada
            object instance = Marshal.PtrToStructure(pointer, typeof(MemoryMappedModule));

            base.ConfigureModule();
        }

        public override void Launch()
        {
            base.Launch();
        }
    }
}
