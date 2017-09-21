namespace Mandelbrot 

open System.Drawing
open Microsoft.FSharp.Collections
open Mandelbrot
open Mandelbrot.Color

type Graph(width:int, height:int, viewPortal:RectangleD, iterations:int) =
    let toPointF (p:PointD) = new PointF(float32 p.X, float32 p.Y)

    let bitmap = new System.Drawing.Bitmap(width, height) 

    let mapPointToPixelPoint (p:PointD) =
        let mappedX = Graph.MapValueToPixel viewPortal.XMin viewPortal.XMax (float width) p.X
        let mappedY = Graph.MapValueToPixel viewPortal.YMin viewPortal.YMax (float height) p.Y
        {PointD.X=mappedX; Y=mappedY}

    let insideBitmap (p:Pixel) =
        p.X < bitmap.Size.Width && p.Y < bitmap.Height && p.X >= 0 && p.Y >= 0

    let pixelMaperX = Graph.MapPixelToValue viewPortal.XMin viewPortal.XMax width
    let pixelMaperY = Graph.MapPixelToValue viewPortal.YMin viewPortal.YMax height
    
    member this.Width = width
    member this.Height = height
    member this.ViewPortal = viewPortal
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
        |> toColor iterations
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
        graphics.DrawLine(p, p1 |> toPointF , p2 |> toPointF )

        let p3 = ({PointD.X=axesLength;PointD.Y=0.} |> mapPointToPixelPoint)
        let p4 = ({PointD.X=(-axesLength);PointD.Y=0.} |> mapPointToPixelPoint)
        graphics.DrawLine(p, p3 |> toPointF, p4 |> toPointF)

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
        |> Seq.map pixelToPixelPointD
        |> PSeq.map (fun (pixel,point) -> (pixel, fn point))
        |> Seq.iter (fun (pixel,v) -> match v with 
                                        | Some(v) -> this.DrawPointAtPixelWithMagnitude pixel v
                                        | None -> this.DrawPointAtPixelWithColor pixel Color.Black)

    static member MapValueToPixel (min:float) (max:float) (pixelWidth:float) (valueToMap:float) =
        ((valueToMap - min) / (max - min)) * pixelWidth

    static member MapPixelToValue (min:float) (max:float) (width:int) (pixelValue:int) = 
        let percent = (float pixelValue) / (float width)
        ((max - min) * percent) + min
