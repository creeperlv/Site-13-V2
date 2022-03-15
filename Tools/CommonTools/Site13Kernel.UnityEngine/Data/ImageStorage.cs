using System.Collections.Generic;

namespace Site13Kernel.Data
{
    public class ImageStorage
    {
        public static RefSprite Default;
        public static Dictionary<string, RefSprite> Sprites = new Dictionary<string, RefSprite>();
        public static RefSprite FindSprite(string Name)
        {
            if (Sprites.ContainsKey(Name)) return Sprites[Name];
            else return Default;
        }
    }
}
