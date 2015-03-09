using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mandala2015.Controls
{
    public class Edge
    {
        public Edge(double x, double y)
        {
            Start = new Point(Math.Round(x), Math.Round(y));
            End = new Point(Math.Round(x), Math.Round(y));
        }
        public Edge(Point start)
            : this(start.X, start.Y)
        {
        }

        public void SetEnd(double x, double y)
        {
            End = new Point(Math.Round(x), Math.Round(y));
        }

        public void SetEnd(Point end)
        {
            SetEnd(Math.Round(end.X), Math.Round(end.Y));
        }

        public Point Start { get; private set; }

        public Point End { get; private set; }
    }
}
