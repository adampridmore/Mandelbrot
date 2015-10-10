open System.Windows.Forms
open System.Drawing
open System
open System.Numerics
open MandelbrotCalc
open RectangleD

[<EntryPoint>]
let main argv = 
//    let graph = new Graph.Graph(300, 200,0.,100.,0.,100.)
//  
//    let picutureBox = new PictureBox()
//    picutureBox.Dock <- DockStyle.Fill
//      
//    let form = new Form()
//    form.Controls.Add(picutureBox)
//
//    let renderGraph() = 
//        let graph = new Graph.Graph(picutureBox.Size.Width, picutureBox.Size.Height,-2.,2.,-2.,2.);
//        graph.DrawAxes()
//        graph |> renderSet
//        graph.Bitmap
//    
//    picutureBox.Image <- renderGraph()
//
//    form.SizeChanged.Add(fun _ -> picutureBox.Image <- renderGraph())
//
//    form.ShowDialog() |> ignore
          
    let render filename (size:System.Drawing.Size) = 

        let viewPortal = {RectangleD.XMin = -2.5; XMax = 1.0; YMin = -1.1; YMax = 1.1}
        let graph = new Graph.Graph(size.Width, size.Height, viewPortal)
        graph |> renderSet  20
        graph.Bitmap.Save(filename)

    let iterationsToCheck = 20

    let size = new System.Drawing.Size(120, 80)
    let filename = sprintf @"C:\temp\mandlebrot\%dx%d-%d.bmp" size.Width size.Height iterationsToCheck

    render filename size
    System.Diagnostics.Process.Start(filename) |> ignore

    0