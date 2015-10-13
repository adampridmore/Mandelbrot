using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MandelbrotWpf
{
    public class ImageCanvas : Canvas
    {
        public ImageSource CanvasImageSource
        {
            get { return (ImageSource)GetValue(CanvasImageSourceProperty); }
            set { SetValue(CanvasImageSourceProperty, value); }
        }

        public static readonly DependencyProperty CanvasImageSourceProperty =
            DependencyProperty.Register("CanvasImageSource", typeof(ImageSource),
            typeof(ImageCanvas), new FrameworkPropertyMetadata(default(ImageSource)));

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawImage(CanvasImageSource, new Rect(RenderSize));
            base.OnRender(dc);
        }
    }
}
