using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Data
{
    public class ImageStorage
    {
        public static Sprite Default;
        public static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public static Sprite FindSprite(string Name)
        {
            if (Sprites.ContainsKey(Name)) return Sprites[Name];
            else return Default;
        }
    }
}
