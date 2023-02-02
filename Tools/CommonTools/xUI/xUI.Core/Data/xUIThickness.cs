using System;
using System.Collections.Generic;
using System.Text;

namespace xUI.Core.Data
{
    [Serializable]
    public class xUIThickness
    {
        public float Top;
        public float Left;
        public float Right;
        public float Bottom;
        public xUIThickness()
        {

        }
        public xUIThickness(float top, float left, float right, float bottom)
        {
            Top = top;
            Left = left;
            Right = right;
            Bottom = bottom;
        }

        public xUIThickness(string data)
        {
            var d = data.Split(',');
            if (d.Length == 1)
            {
                Top = Left = Right = Bottom = float.Parse(d[0]);
            }
            else
            if (d.Length == 2)
            {
                Top = Left = float.Parse(d[0]);
                Right = Bottom = float.Parse(d[1]);
            }
            else if (d.Length == 4)
            {
                Top = float.Parse(d[0]);
                Left = float.Parse(d[1]);
                Right = float.Parse(d[2]);
                Bottom = float.Parse(d[3]);
            }
        }
    }
}
