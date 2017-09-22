using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using Mandelbrot;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using SystemColors = System.Windows.SystemColors;

namespace MandelbrotWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RectangleD _initialViewPort = new RectangleD(-2.5, 1, -1, 1);
        private readonly int _defaultIterations = 100;
        private Point? _startOfDragPosition;

        private readonly Stack<RectangleD> _viewPortHistory = new Stack<RectangleD>();

        private RectangleD CurrentViewPort => _viewPortHistory.Peek();
        private Graph CurrentGraph { get; set; }

        private readonly Rectangle _dragRectangle = new Rectangle();
        private RectangleD _currentSelectedViewPort;

        public MainWindow()
        {
            InitializeComponent();

            comboBox.Items.Add("10");
            comboBox.Items.Add("20");
            comboBox.Items.Add("50");
            comboBox.Items.Add("100");
            comboBox.Items.Add("200");
            comboBox.Items.Add("500");
            comboBox.Items.Add("1000");
            comboBox.Items.Add("5000");

            comboBox.SelectedValue = _defaultIterations.ToString();

            _viewPortHistory.Push(_initialViewPort);

            canvas.Children.Add(_dragRectangle);
            _dragRectangle.Visibility = Visibility.Hidden;
        }

        private void bRefresh_Click(object sender, RoutedEventArgs e)
        {
            RenderMandelbrotSet();
        }

        private void RenderMandelbrotSet()
        {
            using (new WaitCursor())
            {
                if (_currentSelectedViewPort != null)
                {
                    _viewPortHistory.Push(_currentSelectedViewPort);
                    _currentSelectedViewPort = null;
                }

                var size = GetImageSize();

                var iterationsToCheck = GetIterations();


                CurrentGraph = new Graph(
                    size.Width,
                    size.Height,
                    CurrentViewPort,
                    iterationsToCheck);

                MandelbrotCalculator.renderSet(iterationsToCheck, CurrentGraph);
                var imageSource = BitmapToImageSource(CurrentGraph.Bitmap);
                
                canvas.CanvasImageSource = imageSource;

                _dragRectangle.Visibility = Visibility.Hidden;

                canvas.InvalidateVisual();
            }
        }

        private int GetIterations()
        {
            int value;
            if (!int.TryParse(comboBox.Text, out value))
            {
                return _defaultIterations;
            }
            return value;
        }

        private System.Drawing.Size GetImageSize()
        {
            var size = new System.Drawing.Size((int)canvas.ActualWidth, (int)canvas.ActualHeight);
            if (size.Width == 0 || size.Height == 0)
            {
                return new System.Drawing.Size(120, 80);
            }
            else
            {
                return size;
            }
        }
        
        private static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        private void canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startOfDragPosition = e.GetPosition(canvas);
        }

        private void canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_startOfDragPosition.HasValue)
            {
                return;
            }
            
            var endOfDragPosition = e.GetPosition(canvas);

            var startPixel = new Pixel((int)_startOfDragPosition.Value.X, (int)_startOfDragPosition.Value.Y);
            var startValue = CurrentGraph.GetValueFromPixel(startPixel);

            var endPixel = new Pixel((int)endOfDragPosition.X, (int)endOfDragPosition.Y);
            var endValue = CurrentGraph.GetValueFromPixel(endPixel);

            _currentSelectedViewPort = new RectangleD(startValue.X, endValue.X, startValue.Y, endValue.Y);
            _startOfDragPosition = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RenderMandelbrotSet();
        }

        private void bBack_Click(object sender, RoutedEventArgs e)
        {
            if (_viewPortHistory.Count <= 1)
            {
                return;
            }

            _viewPortHistory.Pop();

            RenderMandelbrotSet();
        }

        private void bCopyTransformations_Click(object sender, RoutedEventArgs e)
        {
            var textLines = _viewPortHistory
                .ToArray()
                .Reverse()
                .Select(v=>v.ToString());

            var txt = string.Join(Environment.NewLine, textLines);

            Clipboard.SetText(txt);
        }

        public double Min(double a, double b)
        {
            return a < b ? a : b;
        }

        private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!_startOfDragPosition.HasValue)
            {
                return;
            }
            
            var mousePosition = e.GetPosition(canvas);

            var x1 = _startOfDragPosition.Value.X;
            var y1 = _startOfDragPosition.Value.Y;
            var x2 = mousePosition.X;
            var y2 = mousePosition.Y;

            _dragRectangle.Stroke = SystemColors.WindowFrameBrush;
            _dragRectangle.Width = Math.Abs(x2 - x1);
            _dragRectangle.Height = Math.Abs(y2 - y1);

            _dragRectangle.SetValue(Canvas.LeftProperty, Min(x1, x2));
            _dragRectangle.SetValue(Canvas.TopProperty, Min(y1, y2));

            _dragRectangle.Visibility = Visibility.Visible;
        }

        private void bClearSelection_Click(object sender, RoutedEventArgs e)
        {
            _currentSelectedViewPort = null;
            _dragRectangle.Visibility = Visibility.Hidden;
        }
    }
}