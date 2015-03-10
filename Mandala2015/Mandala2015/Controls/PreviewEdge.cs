using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mandala2015.Controls
{
    public class PreviewEdge : Control
    {
        private Point? start;
        public double ScaledWidth
        {
            get { return (double)GetValue(ScaledWidthProperty); }
            set { SetValue(ScaledWidthProperty, value); }
        }

        public static readonly DependencyProperty ScaledWidthProperty =
            DependencyProperty.Register("ScaledWidth", typeof(double), typeof(PreviewEdge), new PropertyMetadata(30d));

        public double ScaledHeight
        {
            get { return (double)GetValue(ScaledHeightProperty); }
            set { SetValue(ScaledHeightProperty, value); }
        }

        public static readonly DependencyProperty ScaledHeightProperty =
            DependencyProperty.Register("ScaledHeight", typeof(double), typeof(PreviewEdge), new PropertyMetadata(30d));

        public IEnumerable<Point> Preview
        {
            get { return (IEnumerable<Point>)GetValue(PreviewProperty); }
            set { SetValue(PreviewProperty, value); }
        }

        public static readonly DependencyProperty PreviewProperty =
            DependencyProperty.Register("Preview", typeof(IEnumerable<Point>), typeof(PreviewEdge), new PropertyMetadata(null));

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            start = e.GetPosition(this);
            Preview = new[] { ScaleIn(start.Value) };

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Preview = null;
            start = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton == MouseButtonState.Released)
            {
                Preview = null;
                start = null;

            }
            else if (start.HasValue)
            {
                var position = e.GetPosition(this);

                var sizeEdge = Math.Min(ActualHeight, ActualWidth) + 5;

                if (position.X >= -5 && position.Y >= -5 && position.X <= sizeEdge && position.Y <= sizeEdge)
                {
                    Preview = new[] { ScaleIn(start.Value), ScaleIn(position) };
                }
                else
                {
                    Preview = null;
                    start = null;
                }
            }
        }
        private Point ScaleIn(Point p)
        {
            var width = ActualWidth;
            var height = ActualHeight;

            if (ScaledWidth == ScaledHeight)
            {
                width = height = Math.Min(ActualHeight, ActualWidth);
            }
            return new Point(p.X * ScaledWidth / width, p.Y * ScaledHeight / height);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), null, new Rect(new Size(ActualWidth, ActualHeight)));

        }
    }
}
