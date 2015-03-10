using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Mandala2015.Controls
{
    public class DrawingEdge : Control
    {
        private static readonly Pen BlackPen = new Pen(Brushes.Black, 1);

        public double ScaledWidth
        {
            get { return (double)GetValue(ScaledWidthProperty); }
            set { SetValue(ScaledWidthProperty, value); }
        }

        public static readonly DependencyProperty ScaledWidthProperty =
            DependencyProperty.Register("ScaledWidth", typeof(double), typeof(DrawingEdge), new PropertyMetadata(30d, OnDrawingChanged));

        public double ScaledHeight
        {
            get { return (double)GetValue(ScaledHeightProperty); }
            set { SetValue(ScaledHeightProperty, value); }
        }

        public static readonly DependencyProperty ScaledHeightProperty =
            DependencyProperty.Register("ScaledHeight", typeof(double), typeof(DrawingEdge), new PropertyMetadata(30d, OnDrawingChanged));


        public IEnumerable<Point> Drawing
        {
            get { return (IEnumerable<Point>)GetValue(DrawingProperty); }
            set { SetValue(DrawingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Drawing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawingProperty =
            DependencyProperty.Register("Drawing", typeof(IEnumerable<Point>), typeof(DrawingEdge), new PropertyMetadata(null, OnDrawingChanged));

        public Pen Pen
        {
            get { return (Pen)GetValue(PenProperty); }
            set { SetValue(PenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenProperty =
            DependencyProperty.Register("Pen", typeof(Pen), typeof(DrawingEdge), new PropertyMetadata(BlackPen, OnDrawingChanged));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            DrawEdges(drawingContext);
        }

        private Point ScaleOut(Point p)
        {
            var width = ActualWidth;
            var height = ActualHeight;

            if (ScaledWidth == ScaledHeight)
            {
                width = height = Math.Min(ActualHeight, ActualWidth);
            }
            return new Point(Math.Round(p.X * width / ScaledWidth), Math.Round(p.Y * height / ScaledHeight));
        }

        private void DrawEdges(DrawingContext drawingContext)
        {
            if (Drawing != null)
            {
                var drawing = Drawing.Select(ScaleOut).ToList();

                var guidelines = new GuidelineSet();

                foreach (var point in drawing)
                {
                    guidelines.GuidelinesX.Add(point.X + Pen.Thickness / 2);
                    guidelines.GuidelinesY.Add(point.Y + Pen.Thickness / 2);
                }

                drawingContext.PushGuidelineSet(guidelines);

                if (drawing.Count % 2 == 0)
                {
                    for (var i = 0; i < drawing.Count; i += 2)
                    {
                        drawingContext.DrawLine(BlackPen, drawing[i], drawing[i + 1]);
                    }
                }

                if (Background != null && Background != Brushes.Transparent)
                {
                    foreach (var point in drawing)
                    {
                        drawingContext.DrawEllipse(Background, Pen, point, 3, 3);
                    }
                }

                drawingContext.Pop();
            }
        }

        private static void OnDrawingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (DrawingEdge)d;
            self.InvalidateVisual();
        }
    }
}
