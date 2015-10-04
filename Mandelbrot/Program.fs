open System.Windows.Forms
open System.Drawing

[<EntryPoint>]
let main argv = 
    let graph = new Graph.Graph(300, 200,0.,100.,0.,100.)
  
    let picutureBox = new PictureBox()
    picutureBox.Dock <- DockStyle.Fill
      
    let form = new Form()
    form.Controls.Add(picutureBox)

    let graph = new Graph.Graph(picutureBox.Size.Width, picutureBox.Size.Height,-10.,10.,-10.,10.);
    graph.DrawAxes()

    seq{(-50.)..(0.1)..(50.)}
    |> Seq.map(fun x -> x, x*x)
    |> Seq.iter (fun (x,y) -> graph.DrawPoint({X=x; Y=y}))
    
    picutureBox.Image <- graph.Bitmap

    form.ShowDialog() |> ignore
          
    0