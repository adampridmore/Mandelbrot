module Graph
open System.Drawing

type Graph(width:int, height:int, minX:double, maxX:double, minY:double, maxY:double) =
    let bitmap = new Bitmap(width, height)
    
    member this.Width = width
    member this.Height = height
    member this.MinX = minX
    member this.MinY = minY
    member this.MaxY = maxY
    member this.Bitmap = bitmap
    member this.DrawLine point = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let p = new Pen(new SolidBrush(Color.Red),5.f);
        graphics.DrawLine(p, 0,0, 100, 100)
