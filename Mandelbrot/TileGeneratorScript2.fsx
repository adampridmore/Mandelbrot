#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.dll"
#r @"..\PaddingtonRepository\bin\Debug\PaddingtonRepository.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections
open PaddingtonRepository.Domain

let tileSize = 256
let iterations = 400
//let iterations = 50

let mongoUri = @"mongodb://localhost/tiles"

let imageFormat = System.Drawing.Imaging.ImageFormat.Png

type TileDetails = {
        X: int;
        Y: int;
        Zoom: int;
        Filename: string
    }

let repository = new PaddingtonRepository.TileRepository(mongoUri)

let toDomainTile (tile:TileDetails) (graph:Graph) = 
    let domainTile = Tile.CreateTile(tile.X, tile.Y, tile.Zoom, iterations, Tile.MandelbrotSetName)
    
    domainTile.Id <- tile.Filename
    let ms = new System.IO.MemoryStream()
    graph.Bitmap.Save(ms, imageFormat)
    domainTile.Data <- ms.ToArray()
    domainTile

let render iterationsToCheck (tile:TileDetails) (size:System.Drawing.Size) viewPort =     
    let graph = new Graph(size.Width, size.Height, viewPort)
    graph |> renderSet iterationsToCheck

    graph |> toDomainTile tile |> repository.Save

// tile index max = 2^zoom - 1

let size = new System.Drawing.Size(tileSize, tileSize)

let fullViewport = {XMin = -2.0; XMax = 2.0; YMin = -2.; YMax = 2.}

let zoomToCellCount zoom =
    (2. ** (zoom |> float)) |> float

let toRectangleD (tile:TileDetails) =
    let cellCountSize = zoomToCellCount tile.Zoom
    let cellWidth = (fullViewport.XMax - fullViewport.XMin) / cellCountSize
    let cellHeight = (fullViewport.YMax - fullViewport.YMin) / cellCountSize

    {
        XMin = (fullViewport.XMin + (cellWidth * (float tile.X) ) );
        XMax = (fullViewport.XMin + (cellWidth * (float tile.X + 1.) ) );
        YMin = (fullViewport.YMin + (cellHeight * (float tile.Y) ));
        YMax = (fullViewport.YMin + (cellHeight * (float tile.Y + 1.) ) )
    }
    
let toFilename x y zoom = sprintf @"tile_zm%d_x%d_y%d.%A" zoom x y imageFormat

let renderCell (tile:TileDetails) = 
    toRectangleD tile
    |> render iterations tile size 

    printfn "%s" tile.Filename

let tileNeedToBeRendered (tile:TileDetails) = 
    repository.DoesTileExist(tile.X, tile.Y, tile.Zoom, Tile.MandelbrotSetName) |> not

let renderZoomLevel zoom = 
    let cellCount = zoom |> zoomToCellCount |> int
    seq{
        for x in 0 .. (cellCount - 1) do
            for y in 0 .. (cellCount - 1) do
                yield (x,y)
    }
    |> Seq.map (fun (x,y) -> {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom})
    //|> Seq.rev
    |> PSeq.filter (fun tile -> tile |> tileNeedToBeRendered)
    |> PSeq.iter (fun tile -> renderCell tile)
    //|> Seq.iter (printf "%A")
    

//renderZoomLevel 0
//renderZoomLevel 1
//renderZoomLevel 2
//renderZoomLevel 3
//renderZoomLevel 4
//renderZoomLevel 5

#time "on"

//seq{0..2} |> Seq.iter renderZoomLevel
//renderZoomLevel 1
renderZoomLevel 2
//renderZoomLevel 5
//renderZoomLevel 6

//renderZoomLevel 7
//renderZoomLevel 8
// renderZoomLevel 9

// renderZoomLevel 10
// renderZoomLevel 11

