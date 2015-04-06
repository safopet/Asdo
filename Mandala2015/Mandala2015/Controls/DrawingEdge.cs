using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Mandala2015.Controls
{
    internal class DrawingEdge : Control
    {
        private static readonly Pen BlackPen = new Pen(Brushes.Black, 1);

		private double _scaleOutX = 1d;
		private double _scaleOutY = 1d;

        public double ScaledWidth
        {
            get { return (double)GetValue(ScaledWidthProperty); }
            set { SetValue(ScaledWidthProperty, value); }
        }

        public static readonly DependencyProperty ScaledWidthProperty =
            DependencyProperty.Register("ScaledWidth", typeof(double), typeof(DrawingEdge), new PropertyMetadata(30d, UpdateScaledSize));

        public double ScaledHeight
        {
            get { return (double)GetValue(ScaledHeightProperty); }
            set { SetValue(ScaledHeightProperty, value); }
        }

        public static readonly DependencyProperty ScaledHeightProperty =
            DependencyProperty.Register("ScaledHeight", typeof(double), typeof(DrawingEdge), new PropertyMetadata(30d, UpdateScaledSize));


        public IEnumerable<Edge> Drawing
        {
            get { return (IEnumerable<Edge>)GetValue(DrawingProperty); }
            set { SetValue(DrawingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Drawing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawingProperty =
            DependencyProperty.Register("Drawing", typeof(IEnumerable<Edge>), typeof(DrawingEdge), new PropertyMetadata(null, OnDrawingChanged));

        public Pen Pen
        {
            get { return (Pen)GetValue(PenProperty); }
            set { SetValue(PenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenProperty =
            DependencyProperty.Register("Pen", typeof(Pen), typeof(DrawingEdge), new PropertyMetadata(BlackPen, OnDrawingChanged));

		protected static void UpdateScaledSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var drawingEdge = (DrawingEdge)d;
			drawingEdge.InvalidateVisual();
			drawingEdge.UpdateScaleOut();
		}

		protected static void OnDrawingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var drawingEdge = (DrawingEdge)d;
			drawingEdge.InvalidateVisual();
		}

		protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            DrawEdges(drawingContext);
        }

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);

			UpdateScaleOut();
		}

		private void UpdateScaleOut()
		{
			var width = ActualWidth;
			var height = ActualHeight;

			if (ScaledWidth == ScaledHeight)
			{
				width = height = Math.Min(ActualHeight, ActualWidth);
			}
			_scaleOutX = width / ScaledWidth;
			_scaleOutY = height / ScaledHeight;
		}

		private Edge ScaleOut(Edge p)
		{
			return new Edge(
				p.Start.X * _scaleOutX,
				p.Start.Y * _scaleOutY,
				p.End.X * _scaleOutX,
				p.End.Y * _scaleOutY);
		}

		private void DrawEdges(DrawingContext drawingContext)
        {
            if (Drawing != null)
            {
                var drawing = Drawing.Select(ScaleOut).ToList();
				var points = drawing.SelectMany(e => new[] { e.Start, e.End }).ToList();

				var guidelines = new GuidelineSet();

                foreach (var point in points)
                {
                    guidelines.GuidelinesX.Add(point.X + Pen.Thickness / 2);
                    guidelines.GuidelinesY.Add(point.Y + Pen.Thickness / 2);
                }

                drawingContext.PushGuidelineSet(guidelines);

				foreach (var edge in drawing)
				{
					drawingContext.DrawLine(Pen, edge.Start, edge.End);
				}

                if (Background != null && Background != Brushes.Transparent)
                {
                    foreach (var point in points)
                    {
                        drawingContext.DrawEllipse(Background, Pen, point, 3, 3);
                    }
                }

                drawingContext.Pop();
            }
        }

    }
}
