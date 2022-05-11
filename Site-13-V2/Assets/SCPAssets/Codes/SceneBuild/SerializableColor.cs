using System;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class SerializableColor
    {
        public float R;
        public float G;
        public float B;
        public float A;
        public SerializableColor(float r,float g,float b,float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public static implicit operator Color(SerializableColor c)
        {
            return new Color(c.R, c.G, c.B, c.A);
        }
        public static implicit operator SerializableColor(Color c)
        {
            return new SerializableColor(c.r, c.g, c.b, c.a);
        }
    }
}