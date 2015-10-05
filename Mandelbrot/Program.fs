open System.Windows.Forms
open System.Drawing

[<EntryPoint>]
let main argv = 
    let graph = new Graph.Graph(300, 200,0.,100.,0.,100.)
  
    let picutureBox = new PictureBox()
    picutureBox.Dock <- DockStyle.Fill
      
    let form = new Form()
    form.Controls.Add(picutureBox)

    let renderGraph() = 
        let graph = new Graph.Graph(picutureBox.Size.Width, picutureBox.Size.Height,-5.,5.,-10.,10.);
        graph.DrawAxes()

        seq{(-10.)..(0.01)..(10.)}
        |> Seq.map(fun x -> x, x*x)
        //|> Seq.map(fun x -> x, System.Math.Sin(x) )
        |> Seq.iter (fun (x,y) -> graph.DrawPoint({X=x; Y=y}))
        
        graph.Bitmap
    
    picutureBox.Image <- renderGraph()

    let formSizeChanged _ =  
        picutureBox.Image <- renderGraph()

    form.SizeChanged.Add(formSizeChanged)

    form.ShowDialog() |> ignore
          
    0