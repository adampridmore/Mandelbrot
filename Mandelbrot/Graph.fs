module Graph
open System.Drawing
open PointD

type Graph(width:int, height:int, minX:double, maxX:double, minY:double, maxY:double) =
    let bitmap = new Bitmap(width, height)

    let mapPointToPixelPoint (p:PointD) =
        let mappedX = GraphHelpers.mapToPixelValue minX maxX (float width) p.X
        let mappedY = GraphHelpers.mapToPixelValue maxY minX (float width) p.Y
        {PointD.X=mappedX; Y=mappedY}

    let insideBitmap (p:Point) =
        p.X < bitmap.Size.Width && p.Y < bitmap.Height && p.X > 0 && p.Y > 0

    member this.Width = width
    member this.Height = height
    member this.MinX = minX
    member this.MinY = minY
    member this.MaxY = maxY
    member this.Bitmap = bitmap
    member this.DrawLine (point1:PointF) (point2:PointF) = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let p = new Pen(new SolidBrush(Color.Red),5.f);
        graphics.DrawLine(p, 0,0, this.Width, this.Height)

    member this.DrawPoint(p:PointD) =
        let mappedPoint = (p |> mapPointToPixelPoint).ToPoint
        if insideBitmap(mappedPoint) 
        then this.Bitmap.SetPixel((mappedPoint.X) ,(mappedPoint.Y),Color.Black)
        else ()

    member this.DrawAxes() = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let axesColor = Color.DarkBlue
        let axesLength = 1000.
        let p = new Pen(new SolidBrush(axesColor),1.f);
        
        let p1 = ({X=0.;Y=(-axesLength)} |> mapPointToPixelPoint)
        let p2 = ({X=0.;Y=axesLength} |> mapPointToPixelPoint)
        graphics.DrawLine(p, p1.ToPointF, p2.ToPointF)

        let p3 = ({X=axesLength;Y=0.} |> mapPointToPixelPoint)
        let p4 = ({X=(-axesLength);Y=0.} |> mapPointToPixelPoint)
        graphics.DrawLine(p, p3.ToPointF, p4.ToPointF)

