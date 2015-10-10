using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MabdelbrotForm
{
    public partial class Form1 : Form
    {
        private readonly Stack<Rect> _viewPorts = new Stack<Rect>();
        private Rect CurrentViewPort => _viewPorts.Peek();
        private Graph.Graph _currentGraph;
        private Point? _mouseClickStart = null;

        public Form1()
        {
            InitializeComponent();

            _viewPorts.Push(new Rect(-2, 2, -2, 2));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void RenderMandlebrotSet()
        {
            if (_viewPorts.Count == 0)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(CurrentViewPort.ToString());

            _currentGraph = new Graph.Graph(pictureBox1.Width,
                pictureBox1.Height,
                CurrentViewPort.MinX, CurrentViewPort.MaxX,
                CurrentViewPort.MinY, CurrentViewPort.MaxY);
            //g.DrawAxes();
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                MandelbrotCalc.renderSet(200, _currentGraph);

                pictureBox1.Image = _currentGraph.Bitmap;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            RenderMandlebrotSet();
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

            var startValue = _currentGraph.GetValueFromPixel(Pixel.Pixel.FromPoint(_mouseClickStart.Value));
            var endValue = _currentGraph.GetValueFromPixel(Pixel.Pixel.FromPoint(mousePoint));

            _viewPorts.Push(new Rect(startValue.X,
                                endValue.X,
                                endValue.Y,
                                startValue.Y));

            RenderMandlebrotSet();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
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
            RenderMandlebrotSet();
        }
    }
}
