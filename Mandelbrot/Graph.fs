module Graph
open System.Drawing
open PointD
open Pixel
open Microsoft.FSharp.Collections

type Graph(width:int, height:int, minX:double, maxX:double, minY:double, maxY:double) =
    let bitmap = new System.Drawing.Bitmap(width, height) 

    let mapPointToPixelPoint (p:PointD) =
        let mappedX = GraphHelpers.mapValueToPixel minX maxX (float width) p.X
        let mappedY = GraphHelpers.mapValueToPixel maxY minY (float height) p.Y
        {PointD.X=mappedX; Y=mappedY}

    let insideBitmap (p:Pixel) =
        p.X < bitmap.Size.Width && p.Y < bitmap.Height && p.X >= 0 && p.Y >= 0

    let pixelMaperX = GraphHelpers.mapPixelToValue minX maxX width
    let pixelMaperY = GraphHelpers.mapPixelToValue minY maxY height
    
    member this.Width = width
    member this.Height = height
    member this.MinX = minX
    member this.MaxX = maxX
    member this.MinY = minY
    member this.MaxY = maxY
    member this.Bitmap = bitmap
    member this.DrawLine (point1:Pixel) (point2:Pixel) = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let p = new System.Drawing.Pen(new SolidBrush(Color.Red),5.f);
        graphics.DrawLine(p, 0,0, this.Width, this.Height)

    member this.DrawPoint (p:PointD) =
        (p |> mapPointToPixelPoint).ToPixel |> this.DrawPointAtPixel 

    member this.DrawPointAtPixel (p:Pixel) = this.DrawPointAtPixelWithColor p Color.Black
    member this.DrawPointAtPixelWithMagnitude (p:Pixel) (magnitude:int) =
        magnitude 
        |> ColorModule.toColor 
        |> (this.DrawPointAtPixelWithColor p)
    member this.DrawPointAtPixelWithColor (p:Pixel) (c:Color) =
        if insideBitmap(p) 
        then this.Bitmap.SetPixel((p.X) ,(p.Y),c)
        else ()

    member this.DrawAxes() = 
        use graphics = Graphics.FromImage(this.Bitmap)
        let axesColor = Color.DarkBlue
        let axesLength = 1000.
        let p = new System.Drawing.Pen(new SolidBrush(axesColor),1.f);
        
        let p1 = ({PointD.X=0.;PointD.Y=(-axesLength)} |> mapPointToPixelPoint)
        let p2 = ({PointD.X=0.;PointD.Y=axesLength} |> mapPointToPixelPoint)
        graphics.DrawLine(p, p1.ToPointF, p2.ToPointF)

        let p3 = ({PointD.X=axesLength;PointD.Y=0.} |> mapPointToPixelPoint)
        let p4 = ({PointD.X=(-axesLength);PointD.Y=0.} |> mapPointToPixelPoint)
        graphics.DrawLine(p, p3.ToPointF, p4.ToPointF)

    member this.GetValueFromPixel(pixel:Pixel) = 
        {PointD.X = (pixel.X|>pixelMaperX);Y = (pixel.Y|>pixelMaperY) }

    member this.IterateGraph (fn: PointD -> (int Option )) = 
        let pixelToPixelPointD (pixel:Pixel) = 
            let point = this.GetValueFromPixel(pixel)
            (pixel, point)

        seq{
            for y in 0..this.Height-1 do
                for x in 0..this.Width-1 do
                    yield {Pixel.X=x; Pixel.Y=y}
        }
        |> PSeq.map pixelToPixelPointD
        |> PSeq.map (fun (pixel,point) -> (pixel, fn point))
        |> Seq.iter (fun (pixel,v) -> match v with 
                                      | Some(v) -> this.DrawPointAtPixelWithMagnitude pixel v
                                      | None -> this.DrawPointAtPixelWithColor pixel Color.Black)
