using Site13Kernel.IO.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.UEFI
{
    public class FileSystemBuilder : UEFIBase
    {
        public BaseMap Mapping;
        public override void Init()
        {
            base.Init();
        }
        public override async Task Run()
        {
            await Task.Run(()=> { 
            
            });
        }
    }
}
