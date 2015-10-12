using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mandelbrot;

namespace MabdelbrotForm
{
    public partial class Form1 : Form
    {
        private readonly Stack<RectangleD> _viewPorts = new Stack<RectangleD>();
        private RectangleD CurrentViewPort => _viewPorts.Peek();
        private Graph _currentGraph;
        private Point? _mouseClickStart = null;

        private int _iterations = 200;

        public Form1()
        {
            InitializeComponent();

            this.AutoScaleDimensions = new SizeF(96f, 96f);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            var startViewPort = new RectangleD(
                -0.7603053435,
                -0.5404580153,
                0.3225806452,
                0.5923753666);
            //new RectangleD.RectangleD(-2, 2, -2, 2)

            _viewPorts.Push(startViewPort);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RenderMandlebrotSet();
        }

        private void RenderMandlebrotSet()
        {
            if (_viewPorts.Count == 0)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(CurrentViewPort.ToString());



            _currentGraph = new Graph(pictureBox1.Width,
                pictureBox1.Height,
                CurrentViewPort);
            //g.DrawAxes();
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                MandelbrotCalculator.renderSet(_iterations, _currentGraph);

                pictureBox1.Image = _currentGraph.Bitmap;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //RenderMandlebrotSet();
        }
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            DragBoxStart(e.Location);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            DragBoxEnd(e.Location);
        }

        private void DragBoxStart(Point mousePoint)
        {
            _mouseClickStart = mousePoint;
        }

        private void DragBoxEnd(Point mousePoint)
        {
            if (!_mouseClickStart.HasValue)
            {
                return;
            }

            var newViewPort = GetViewPortFromStartAndCurrentMousePos(mousePoint);

            _viewPorts.Push(newViewPort);

            _mouseClickStart = null;
            tbHoverText.Text = CurrentViewPort.ToString();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseClickStart.HasValue)
            {
                return;
            }

            var newViewPort = GetViewPortFromStartAndCurrentMousePos(e.Location);
            tbHoverText.Text = newViewPort.ToString();
        }

        private Point TranslateMousePoint(Point mousePoint)
        {
            // The mouse is specified in pixel from top, but the view is in pixel from bottom
            return new Point(mousePoint.X, pictureBox1.Height - mousePoint.Y);
        }

        private RectangleD GetViewPortFromStartAndCurrentMousePos(Point mousePoint)
        {
            var startValue = _currentGraph.GetValueFromPixel(Pixel.FromPoint(TranslateMousePoint(_mouseClickStart.Value)));
            var endValue = _currentGraph.GetValueFromPixel(Pixel.FromPoint(TranslateMousePoint(mousePoint)));

            var newViewPort = new RectangleD(startValue.X,
                endValue.X,
                endValue.Y,
                startValue.Y);
            return newViewPort;
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            if (_viewPorts.Count > 1)
            {
                _viewPorts.Pop();
                RenderMandlebrotSet();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void bRender_Click(object sender, EventArgs e)
        {
            RenderMandlebrotSet();
        }
    }
}
