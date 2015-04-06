using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mandala2015.Controls
{
	internal struct Edge
	{
		public Edge(Point start, Point end)
		{
			Start = start;
			End = end;
		}

		public Edge(Point point) 
			: this(point, point)
		{
		}

		public Edge(double sx, double sy, double ex, double ey)
			: this(new Point(sx, sy), new Point(ex, ey))
		{
		}

		public Point Start { get; }

		public Point End { get; }

		public Edge SetEnd(Point end)
		{
			return new Edge(Start, end);
		}
	}
}
