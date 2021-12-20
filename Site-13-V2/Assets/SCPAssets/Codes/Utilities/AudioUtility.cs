using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Site13Kernel.Assets.KoFMUST.Codes.Utilities
{
    public static class AudioUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetVolume(AudioMixer AM,float Volume)
        {
            if (Volume == 0)
            {
                AM.SetFloat("Volume", -80);
            }
            else
            {
                AM.SetFloat("Volume", Mathf.Log10(Volume / 10f) * 20);
            }
        }
    }
}
