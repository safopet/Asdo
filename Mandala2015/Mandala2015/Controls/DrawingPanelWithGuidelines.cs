using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Mandala2015.Controls
{
    public class DrawingPanelWithGuidelines : Control
    {
        private static readonly Pen BlackPen = new Pen(Brushes.Black, 1);


        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(DrawingPanelWithGuidelines), new PropertyMetadata(10, InvokeRendering));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(DrawingPanelWithGuidelines), new PropertyMetadata(10, InvokeRendering));

        public Pen Guideline
        {
            get { return (Pen)GetValue(GuidelineProperty); }
            set { SetValue(GuidelineProperty, value); }
        }

        public static readonly DependencyProperty GuidelineProperty =
            DependencyProperty.Register("Guideline", typeof(Pen), typeof(DrawingPanelWithGuidelines), new PropertyMetadata(BlackPen, InvokeRendering));

        public Pen Axis
        {
            get { return (Pen)GetValue(AxisProperty); }
            set { SetValue(AxisProperty, value); }
        }

        public static readonly DependencyProperty AxisProperty =
            DependencyProperty.Register("Axis", typeof(Pen), typeof(DrawingPanelWithGuidelines), new PropertyMetadata(BlackPen, InvokeRendering));

        public Pen Diagonal
        {
            get { return (Pen)GetValue(DiagonalProperty); }
            set { SetValue(DiagonalProperty, value); }
        }

        public static readonly DependencyProperty DiagonalProperty =
            DependencyProperty.Register("Diagonal", typeof(Pen), typeof(DrawingPanelWithGuidelines), new PropertyMetadata(BlackPen, InvokeRendering));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (Columns == 0 || Rows == 0)
            {
                return;
            }

            var width = ActualWidth;
            var height = ActualHeight;

            if (Columns == Rows)
            {
                width = height = Math.Min(ActualHeight, ActualWidth);
            }

            DrawGuidelines(drawingContext, width, height);

            DrawAxis(drawingContext, width, height);
        }

        private void DrawAxis(DrawingContext drawingContext, double width, double height)
        {
            var guidelines = new GuidelineSet();

            guidelines.GuidelinesX.Add(width / 2 + Axis.Thickness / 2);
            guidelines.GuidelinesY.Add(height / 2 + Axis.Thickness / 2);

            guidelines.GuidelinesX.Add(Diagonal.Thickness / 2);
            guidelines.GuidelinesY.Add(Diagonal.Thickness / 2);
            guidelines.GuidelinesX.Add(width + Diagonal.Thickness / 2);
            guidelines.GuidelinesY.Add(height + Diagonal.Thickness / 2);

            drawingContext.PushGuidelineSet(guidelines);

            drawingContext.DrawLine(Axis, new Point(width / 2, 0), new Point(width / 2, height));
            drawingContext.DrawLine(Axis, new Point(0, height / 2), new Point(width, height / 2));

            drawingContext.DrawLine(Diagonal, new Point(Diagonal.Thickness / 2, Diagonal.Thickness / 2), new Point(width + Diagonal.Thickness / 2, height + Diagonal.Thickness / 2));
            drawingContext.DrawLine(Diagonal, new Point(Diagonal.Thickness / 2, height + 1 + Diagonal.Thickness / 2), new Point(width + 1 + Diagonal.Thickness / 2, Diagonal.Thickness / 2));

            drawingContext.Pop();
        }

        private void DrawGuidelines(DrawingContext drawingContext, double width, double height)
        {
            var guidelines = new GuidelineSet();

            var hStep = width / Columns;
            var vStep = height / Rows;

            for (var i = 0; i <= Columns; i++)
            {
                var s = Math.Round(i * hStep) + Guideline.Thickness / 2;
                guidelines.GuidelinesX.Add(s);
            }
            for (var i = 0; i <= Rows; i++)
            {
                var s = Math.Round(i * vStep) + Guideline.Thickness / 2;
                guidelines.GuidelinesY.Add(s);
            }

            drawingContext.PushGuidelineSet(guidelines);

            for (var i = 0; i <= Columns; i++)
            {
                if (i != Columns / 2)
                {
                    var s = Math.Round(i * hStep);
                    drawingContext.DrawLine(Guideline, new Point(s, 0), new Point(s, height));
                }
            }

            for (var i = 0; i <= Rows; i++)
            {
                if (i != Rows / 2)
                {
                    var s = Math.Round(i * vStep);
                    drawingContext.DrawLine(Guideline, new Point(0, s), new Point(width, s));
                }
            }

            drawingContext.Pop();
        }

        private static void InvokeRendering(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cage = (DrawingPanelWithGuidelines)d;
            cage.InvalidateVisual();
        }
    }
}
