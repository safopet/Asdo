using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mandala2015.Controls
{
	internal class PreviewEdge : Control
	{
		private double _scaleInX = 1d;
		private double _scaleInY = 1d;

		public double ScaledWidth
		{
			get { return (double)GetValue(ScaledWidthProperty); }
			set { SetValue(ScaledWidthProperty, value); }
		}

		public static readonly DependencyProperty ScaledWidthProperty =
			DependencyProperty.Register("ScaledWidth", typeof(double), typeof(PreviewEdge), new PropertyMetadata(30d, UpdateScaledSize));


		public double ScaledHeight
		{
			get { return (double)GetValue(ScaledHeightProperty); }
			set { SetValue(ScaledHeightProperty, value); }
		}

		public static readonly DependencyProperty ScaledHeightProperty =
			DependencyProperty.Register("ScaledHeight", typeof(double), typeof(PreviewEdge), new PropertyMetadata(30d, UpdateScaledSize));

		public Edge? Preview
		{
			get { return (Edge?)GetValue(PreviewProperty); }
			set { SetValue(PreviewProperty, value); }
		}

		public static readonly DependencyProperty PreviewProperty =
			DependencyProperty.Register("Preview", typeof(Edge?), typeof(PreviewEdge), new PropertyMetadata(null));

		protected static void UpdateScaledSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var previewEdge = (PreviewEdge)d;

			previewEdge.UpdateScaleIn();
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);

			UpdateScaleIn();
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			Preview = new Edge(ScaleIn(e.GetPosition(this)));

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			Preview = null;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.LeftButton == MouseButtonState.Released)
			{
				Preview = null;

			}
			else if (Preview.HasValue)
			{
				var position = e.GetPosition(this);

				var sizeEdge = Math.Min(ActualHeight, ActualWidth) + 5;

				if (position.X >= -5 && position.Y >= -5 && position.X <= sizeEdge && position.Y <= sizeEdge)
				{
					Preview = Preview.Value.SetEnd(ScaleIn(position));
				}
				else
				{
					Preview = null;
				}
			}
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			drawingContext.DrawRectangle(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), null, new Rect(new Size(ActualWidth, ActualHeight)));

		}

		private void UpdateScaleIn()
		{
			var width = ActualWidth;
			var height = ActualHeight;

			if (ScaledWidth == ScaledHeight)
			{
				width = height = Math.Min(ActualHeight, ActualWidth);
			}

			_scaleInX = ScaledWidth / width;
			_scaleInY = ScaledHeight / height;
		}

		private Point ScaleIn(Point p)
        {
            return new Point(p.X * _scaleInX, p.Y * _scaleInY);
        }
    }
}
