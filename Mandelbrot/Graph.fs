namespace Mandelbrot

open Mandelbrot
open Mandelbrot.Color
open Mandelbrot.Image2

type Graph(width:int, height:int, viewPortal:RectangleD, iterations:int) =

    let bitmap: Bitmap3 = new Bitmap3(width, height)

    let mapPointToPixelPoint (p:PointD) =
        let mappedX = Graph.MapValueToPixel viewPortal.XMin viewPortal.XMax (float width) p.X
        let mappedY = Graph.MapValueToPixel viewPortal.YMin viewPortal.YMax (float height) p.Y
        {PointD.X=mappedX; Y=mappedY}

    let insideBitmap (p:Pixel) =
        p.X < width && p.Y < height && p.X >= 0 && p.Y >= 0

    let pixelMaperX = Graph.MapPixelToValue viewPortal.XMin viewPortal.XMax width
    let pixelMaperY = Graph.MapPixelToValue viewPortal.YMin viewPortal.YMax height

    member this.Width = width
    member this.Height = height
    member this.ViewPortal = viewPortal
    member this.Bitmap = bitmap

    member this.DrawPoint (p:PointD) =
        (p |> mapPointToPixelPoint).ToPixel |> this.DrawPointAtPixel

    member this.DrawPointAtPixel (p:Pixel) = this.DrawPointAtPixelWithColor p Mandelbrot.Color.Black
    member this.DrawPointAtPixelWithMagnitude (p:Pixel) (magnitude:int) =
        magnitude
        |> toColor iterations
        |> (this.DrawPointAtPixelWithColor p)
    member this.DrawPointAtPixelWithColor (p:Pixel) (c:Color2) =
        if insideBitmap(p)
        then this.Bitmap.setPixel(p.X, p.Y, c)
        else ()

    member this.GetValueFromPixel(pixel:Pixel) =
        {PointD.X = (pixel.X|>pixelMaperX);Y = (pixel.Y|>pixelMaperY) }

    member this.IterateGraph (fn: PointD -> int option) =
        let results =
            Array.Parallel.init (width * height) (fun i ->
                let x = i % width
                let y = i / width
                let pixel = { Pixel.X = x; Pixel.Y = y }
                let point = this.GetValueFromPixel pixel
                pixel, fn point)

        for pixel, v in results do
            match v with
            | Some v -> this.DrawPointAtPixelWithMagnitude pixel v
            | None -> this.DrawPointAtPixelWithColor pixel Black

    member this.IterateGraphWithIterations(iterationResults: int[]) =
        for i in 0 .. (width * height - 1) do
            let px = i % width
            let py = i / width
            let pixel = { Pixel.X = px; Pixel.Y = py }
            let v = iterationResults.[i]
            if v = -1 then
                this.DrawPointAtPixelWithColor pixel Black
            else
                this.DrawPointAtPixelWithMagnitude pixel v

    member this.IterateGraphSequential (fn: PointD -> int option) =
        let results =
            Array.init (width * height) (fun i ->
                let x = i % width
                let y = i / width
                let pixel = { Pixel.X = x; Pixel.Y = y }
                let point = this.GetValueFromPixel pixel
                pixel, fn point)

        for pixel, v in results do
            match v with
            | Some v -> this.DrawPointAtPixelWithMagnitude pixel v
            | None -> this.DrawPointAtPixelWithColor pixel Black

    static member MapValueToPixel (min:float) (max:float) (pixelWidth:float) (valueToMap:float) =
        ((valueToMap - min) / (max - min)) * pixelWidth

    static member MapPixelToValue (min:float) (max:float) (width:int) (pixelValue:int) =
        let percent = (float pixelValue) / (float width)
        ((max - min) * percent) + min
