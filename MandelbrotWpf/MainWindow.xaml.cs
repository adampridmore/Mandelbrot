using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using Point = System.Windows.Point;

namespace MandelbrotWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RectangleD.RectangeD _initialViewPort = new RectangleD.RectangeD(-2.5, 1, -1, 1);
        private readonly int _defaultIterations = 100;
        private Point? _startOfDragPosition;

        private readonly Stack<RectangleD.RectangeD> _viewPortHistory = new Stack<RectangleD.RectangeD>();

        private RectangleD.RectangeD CurrentViewPort => _viewPortHistory.Peek();
        private Graph.Graph CurrentGraph { get; set; }

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
        }

        private void bRefresh_Click(object sender, RoutedEventArgs e)
        {
            RenderMandelbrotSet();
        }

        private void RenderMandelbrotSet()
        {
            using (new WaitCursor())
            {
                var size = GetImageSize();

                CurrentGraph = new Graph.Graph(
                    size.Width,
                    size.Height,
                    CurrentViewPort);

                MandelbrotCalc.renderSet(GetIterations(), CurrentGraph);
                image.Source = BitmapToImageSource(CurrentGraph.Bitmap);
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
            var size = new System.Drawing.Size((int)image.ActualWidth, (int)image.ActualHeight);
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

        private void image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startOfDragPosition = e.GetPosition(image);
        }

        private void image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_startOfDragPosition.HasValue)
            {
                return;
            }

            if (CurrentViewPort == null)
            {
                return;
            }

            var endOfDragPosition = e.GetPosition(image);

            var startPixel = new Pixel.Pixel((int)_startOfDragPosition.Value.X, (int)_startOfDragPosition.Value.Y);
            var startValue = CurrentGraph.GetValueFromPixel(startPixel);

            var endPixel = new Pixel.Pixel((int)endOfDragPosition.X, (int)endOfDragPosition.Y);
            var endValue = CurrentGraph.GetValueFromPixel(endPixel);

            _viewPortHistory.Push(new RectangleD.RectangeD(startValue.X, endValue.X, startValue.Y, endValue.Y));

            RenderMandelbrotSet();
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
    }
}