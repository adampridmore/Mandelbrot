module Graph
open System.Drawing
open PointD
open Microsoft.FSharp.Collections
open MandelbrotCalc

type Graph(width:int, height:int, minX:double, maxX:double, minY:double, maxY:double) =
    let bitmap = new Bitmap(width, height)

    let mapPointToPixelPoint (p:PointD) =
        let mappedX = GraphHelpers.mapToPixelValue minX maxX (float width) p.X
        let mappedY = GraphHelpers.mapToPixelValue maxY minY (float height) p.Y
        {PointD.X=mappedX; Y=mappedY}

    let insideBitmap (p:Point) =
        p.X < bitmap.Size.Width && p.Y < bitmap.Height && p.X > 0 && p.Y > 0

    let pixelMaperX = GraphHelpers.pixelToValue minX maxX width
    let pixelMaperY = GraphHelpers.pixelToValue maxY minY height

    member this.Width = width
    member this.Height = height
    member this.MinX = minX
    member this.MaxX = maxX
    member this.MinY = minY
    member this.MaxY = maxY
    member this.Bitmap = bitmap
    member this.DrawLine (point1:PointF) (point2:PointF) = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let p = new Pen(new SolidBrush(Color.Red),5.f);
        graphics.DrawLine(p, 0,0, this.Width, this.Height)

    member this.DrawPoint (p:PointD) =
        (p |> mapPointToPixelPoint).ToPoint |> this.DrawPointAtPixel 

    member this.DrawPointAtPixel (p:Point) = this.DrawPointAtPixelWithColor p Color.Black
    member this.DrawPointAtPixelWithMagnitude (p:Point) (magnitude:int) =
        //Color.FromArgb(255-magnitude%255,0,255-magnitude%255)
        magnitude 
        |> ColorModule.toColor 
        |> (this.DrawPointAtPixelWithColor p)
    member this.DrawPointAtPixelWithColor (p:Point) (c:Color) =
        if insideBitmap(p) 
        then this.Bitmap.SetPixel((p.X) ,(p.Y),c)
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

    member this.GetValueFromPixel(pixel:Point) = 
        {PointD.X=(pixel.X|>pixelMaperX);Y=(pixel.Y|>pixelMaperY)}

    member this.IterateGraph (fn: float-> float -> (int Option )) = 
        let pixelToPixelPointD (pixel:Point) = 
            (pixel, this.GetValueFromPixel(pixel))

        seq{
            for x in 0..this.Width do 
                for y in 0..this.Height do
                    yield new Point(x,y)
        }
        |> PSeq.map pixelToPixelPointD
        |> PSeq.map (fun (pixel,point) -> (pixel, fn point.X point.Y))
        |> PSeq.filter (fun (_,v) -> v.IsSome)
        |> Seq.iter (fun (pixel,v) -> this.DrawPointAtPixelWithMagnitude pixel v.Value)

    member this.RenderSet() = 
        this.IterateGraph fn
