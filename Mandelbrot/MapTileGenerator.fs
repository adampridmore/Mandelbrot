module MapTileGenerator

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections

let tileSize = 256
let iterations = 200

let imageFormat = System.Drawing.Imaging.ImageFormat.Png

let render iterationsToCheck (size:System.Drawing.Size) viewPort =     
    let graph = new Graph(size.Width, size.Height, viewPort)
    graph |> renderSet iterationsToCheck
    
    let ms = new System.IO.MemoryStream()
    graph.Bitmap.Save(ms, imageFormat)
    ms

let size = new System.Drawing.Size(tileSize, tileSize)

let fullViewport = {XMin = -2.0; XMax = 2.0; YMin = -2.; YMax = 2.}

let zoomToCellCount zoom =
    (2. ** (zoom |> float)) |> float

let toRectangleD (x:int) (y:int) (z:int) =
    let cellCountSize = zoomToCellCount z
    let cellWidth = (fullViewport.XMax - fullViewport.XMin) / cellCountSize
    let cellHeight = (fullViewport.YMax - fullViewport.YMin) / cellCountSize

    {
        XMin = (fullViewport.XMin + (cellWidth * (float x) ) );
        XMax = (fullViewport.XMin + (cellWidth * (float x + 1.) ) );
        YMin = (fullViewport.YMin + (cellHeight * (float y) ));
        YMax = (fullViewport.YMin + (cellHeight * (float y + 1.) ) )
    }
    

let renderCell x y zoom = 
    toRectangleD x y zoom
    |> render iterations size


