using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTNodeEditor.Editors.Nodes
{
    [Serializable]
    public class NodeConnection
    {
        private Point start;
        private Point end;
        Path? _path = null;
        private GraphNode? l;
        private GraphNode? r;
        public Point Start
        {
            get => start; set
            {
                start = value;
                CalculatePath();
            }
        }
        public Point End
        {
            get => end; set
            {
                end = value;
                CalculatePath();
            }
        }

        public GraphNode? R { get => r; set { r = value; CalculatePath(); } }
        public GraphNode? L { get => l; set { l = value; CalculatePath(); } }

        public void CalculatePath()
        {
            var p = GetPath();
            Point start = this.start;
            Point end = this.end;
            if (L != null)
            {
                start = new Point(L.Margin.Left + L.NodeBorder.DesiredSize.Width, L.Margin.Top+15);
            }
            if (R != null)
            {
                end = new Point(R.Margin.Left, R.Margin.Top + 15);
            }
            try
            {
                var D = end - start;
                var G = $"M {start.X},{start.Y} Q {start.X + Math.Abs(D.X) /3},{start.Y} {start.X + (D.X) / 2},{start.Y + (D.Y) / 2} {end.X - Math.Abs(D.X) / 3},{end.Y} {end.X},{end.Y}";
                p.Data = 
                    Geometry.Parse(G);
            }
            catch (Exception)
            {
            }
        }
        static SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x88, 0x88, 0x88));
        public Path GetPath()
        {
            if (_path is null)
            {
                _path = new Path();
                _path.Stroke = brush;
                _path.StrokeThickness = 1;
            }
            return _path;
        }
    }
}
