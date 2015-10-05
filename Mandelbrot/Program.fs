open System.Windows.Forms
open System.Drawing
open System
open System.Numerics

[<EntryPoint>]
let main argv = 
    let graph = new Graph.Graph(300, 200,0.,100.,0.,100.)
  
    let picutureBox = new PictureBox()
    picutureBox.Dock <- DockStyle.Fill
      
    let form = new Form()
    form.Controls.Add(picutureBox)

    let renderGraph() = 
        let graph = new Graph.Graph(picutureBox.Size.Width, picutureBox.Size.Height,-2.,2.,-2.,2.);
        graph.DrawAxes()
        
        let sqr x = x*x
        //let fn x y = (System.Math.Sqrt (x*x + y*y)) < 1.
//        let fn (x:float) (y:float) = ((x |> sqr) + ( ( ( (5.*y) / 4.) - Math.Sqrt(Math.Abs(x))) |> sqr) ) < 1.
        let fn x y = 
            let c = new Complex(x,y)
            MandelbrotCalc.inSet c

        graph.IterateGraph fn
        
        graph.Bitmap
    
    picutureBox.Image <- renderGraph()

    let formSizeChanged _ = picutureBox.Image <- renderGraph()

    form.SizeChanged.Add(formSizeChanged)
    form.ShowDialog() |> ignore
          
    0