open System.Windows.Forms
open System.Drawing

[<EntryPoint>]
let main argv = 
    let graph = new Graph.Graph(300, 200,0.,100.,0.,100.)
  
    let picutureBox = new PictureBox()
    picutureBox.Dock <- DockStyle.Fill
  
    let graph = new Graph.Graph(100,100,0.,0.,100.,100.);

    graph.DrawLine ()
    picutureBox.Image <- graph.Bitmap

    picutureBox.BorderStyle <- BorderStyle.Fixed3D

    let form = new Form()
    form.Controls.Add(picutureBox)

    form.ShowDialog() |> ignore
          
    0