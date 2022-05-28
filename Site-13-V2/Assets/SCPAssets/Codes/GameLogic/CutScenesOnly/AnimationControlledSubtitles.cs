using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class AnimationControlledSubtitles : MonoBehaviour
    {
        public List<Subtitle> AvailableSubtitle;
        public void ShowSubtitle(int id)
        {
            GameRuntime.CurrentGlobals.SubtitleController.ShowSubtitle(AvailableSubtitle[id]);
        }
    }
}
