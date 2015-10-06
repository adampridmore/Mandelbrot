open System.Windows.Forms
open System.Drawing
open System
open System.Numerics
open MandelbrotCalc

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
        graph.RenderSet()
        graph.Bitmap
    
    picutureBox.Image <- renderGraph()

    form.SizeChanged.Add(fun _ -> picutureBox.Image <- renderGraph())

    form.ShowDialog() |> ignore
          
    0