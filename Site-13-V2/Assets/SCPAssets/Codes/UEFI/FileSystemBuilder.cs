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
            ApplicationData.Init(Mapping);
        }
        public override async Task Run()
        {
            await Task.Run(() =>
            {

            });
        }
    }
}
