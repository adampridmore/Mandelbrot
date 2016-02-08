#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.2.0.0.0\Lib\Net40\FSharp.PowerPack.dll"


#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections

let tileSize = 256
//let iterations = 200
let iterations = 50

let imageFormat = System.Drawing.Imaging.ImageFormat.Png

let render iterationsToCheck (size:System.Drawing.Size) (filename:string) viewPort =     
    let graph = new Graph(size.Width, size.Height, viewPort)
    graph |> renderSet iterationsToCheck

    graph.Bitmap.Save(filename, imageFormat)

// tile index max = 2^zoom - 1

let size = new System.Drawing.Size(tileSize, tileSize)

let fullViewport = {XMin = -2.0; XMax = 2.0; YMin = -2.; YMax = 2.}

//render iterations size @"C:\temp\mandlebrot\tile_0_0_0"  fullViewport
//
//render iterations size @"C:\temp\mandlebrot\tile_0_0_1" {XMin = -2.;  XMax = 0.;  YMin =  -2. ; YMax =  0.}
//render iterations size @"C:\temp\mandlebrot\tile_1_0_1" {XMin = 0.;  XMax = 2.;  YMin =  -2. ; YMax =  0.}
//render iterations size @"C:\temp\mandlebrot\tile_0_1_1" {XMin = -2.;  XMax = 0.;  YMin = 0. ; YMax =  2.}
//render iterations size @"C:\temp\mandlebrot\tile_1_1_1" {XMin =  0.;  XMax = 2.;  YMin =  0. ; YMax =  2.}

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
    
//toRectangleD 0 0 0
//
//toRectangleD 0 0 1
//toRectangleD 1 0 1
//toRectangleD 0 1 1
//toRectangleD 1 1 1

let detinationFolder = @"C:\temp\mandelbrot"

let toFilename x y zoom = sprintf @"%s\tile_zm%d_x%d_y%d.%A" detinationFolder zoom x y imageFormat

let renderCell filename x y zoom = 
    toRectangleD x y zoom
    |> render iterations size filename 

    printfn "%s" filename

let existingTiles = 
    System.IO.Directory.GetFiles(detinationFolder)
    |> Seq.map System.IO.Path.GetFileName
    

type Tile = {
        X: int;
        Y: int;
        Filename: string
    }

let filterExistingTiles existingTiles (tilesToRender: (Tile seq) ) =
    tilesToRender 
    |> Seq.filter (fun tile -> existingTiles |> Seq.exists (fun filename -> filename = tile.Filename) |> not)


let renderZoomLevel zoom = 
    let cellCount = zoom |> zoomToCellCount |> int
    seq{
        for x in 0 .. (cellCount - 1) do
            for y in 0 .. (cellCount - 1) do
                yield (x,y)
    }
    |> Seq.map (fun (x,y) -> {X=x;Y=y;Filename=(toFilename x y zoom)})
    |> Seq.filter (fun tile -> System.IO.File.Exists tile.Filename |> not)
    //|> filterExistingTiles existingTiles
    |> Seq.rev
    |> PSeq.iter (fun tile -> renderCell tile.Filename tile.X tile.Y zoom)
    //|> Seq.iter (printf "%A")
    

System.IO.File.Exists @"C:\temp\mandelbrot\tile_zm1_x0_y0"
//renderZoomLevel 0
//renderZoomLevel 1
//renderZoomLevel 2
//renderZoomLevel 3
//renderZoomLevel 4
//renderZoomLevel 5

#time "on"

//seq{0..2} |> Seq.iter renderZoomLevel
//renderZoomLevel 1
//renderZoomLevel 5
//renderZoomLevel 6

//renderZoomLevel 7
//renderZoomLevel 8
renderZoomLevel 9


//renderZoomLevel 10
//renderZoomLevel 11

