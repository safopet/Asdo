using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Mandala2015.Controls
{
    public class DrawingEdge : ContentControl
    {
        private static readonly Pen BlackPen = new Pen(Brushes.Black, 1);

        private Edge drawingEdge = null;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            var position = e.GetPosition(this);
            drawingEdge = new Edge(position);

            InvalidateVisual();
            base.OnMouseDown(e);
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            drawingEdge = null;
            InvalidateVisual();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (drawingEdge != null)
            {
                var position = e.GetPosition(this);
                drawingEdge.SetEnd(position);
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(0,0,0,0)), BlackPen, new Rect(new Size(ActualWidth, ActualHeight)));

            if (drawingEdge != null)
            {
                var guidelines = new GuidelineSet();
                guidelines.GuidelinesX.Add(drawingEdge.Start.X + 0.5);
                guidelines.GuidelinesY.Add(drawingEdge.Start.Y + 0.5);
                guidelines.GuidelinesX.Add(drawingEdge.End.X + 0.5);
                guidelines.GuidelinesY.Add(drawingEdge.End.Y + 0.5);

                drawingContext.DrawLine(BlackPen, drawingEdge.Start, drawingEdge.End);
            }
        }
    }
}
