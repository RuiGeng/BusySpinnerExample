namespace BusySpinnerExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    public class BusySpinner
    {
        private const int CanvasHeight = 240;
        private const int CanvasWidth = 240;
        private const int Steps = 16;
        private DispatcherTimer animationTimer;
        private Brush brush;

        public BusySpinner(Brush brush)
        {
            this.brush = brush;
            this.BusySpinnerCanvas = CreateBusySpinner(this.brush);
            this.animationTimer = new DispatcherTimer(DispatcherPriority.Normal);
            this.animationTimer.Interval = TimeSpan.FromMilliseconds(50);
            animationTimer.Tick += animationTimer_Tick;
        }

        public Canvas BusySpinnerCanvas { get; }

        public void AnimationStart()
        {
            animationTimer.Start();
        }

        public void AnimationStop()
        {
            animationTimer.Stop();
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            foreach (Polygon p in BusySpinnerCanvas.Children)
            {
                if (p.Opacity <= 0)
                {
                    p.Opacity = 1.0;
                }
                p.Opacity = p.Opacity - 0.05;
            }
        }

        private Canvas CreateBusySpinner(Brush brush)
        {
            Canvas canvas = new Canvas
            {
                Background = Brushes.Transparent,
                Width = CanvasWidth,
                Height = CanvasHeight
            };

            Point[] templatePoints = new[]
            {
                new Point(-10, -120),
                new Point(5, -120),
                new Point(5, -40),
                new Point(-10, -40)
            };

            Matrix m = Matrix.Identity;
            double anglePerStep = 360.0 / Steps;
            List<Polygon> polygons = new List<Polygon>();

            for (int iStep = 0; iStep < Steps; iStep++)
            {
                Polygon p = new Polygon
                {
                    Fill = brush,
                    Opacity = (double)iStep / Steps,
                    Points = new PointCollection(templatePoints.Select(m.Transform))
                };

                Canvas.SetLeft(p, CanvasWidth / 2);
                Canvas.SetTop(p, CanvasHeight / 2);

                polygons.Add(p);
                m.Rotate(anglePerStep);
            }

            polygons.ForEach(
                p => canvas.Children.Add(p)
                );

            return canvas;
        }
    }
}