module MapTileGenerator

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections
open PaddingtonRepository.Domain

let tileSize = 256
let iterations = 400

let private mongoUri = (System.Configuration.ConfigurationManager.ConnectionStrings.["MongoDB"].ConnectionString)

let imageFormat = System.Drawing.Imaging.ImageFormat.Png

type TileDetails = {
        X: int;
        Y: int;
        Zoom: int;
        Filename: string
    }

let private repository = new PaddingtonRepository.TileRepository(mongoUri)

let private toDomainTile (tile:TileDetails) (graph:Graph) = 
    let domainTile = Tile.CreateTile(tile.X, tile.Y, tile.Zoom, iterations, Tile.MandelbrotSetName)
    
    domainTile.Id <- tile.Filename
    let ms = new System.IO.MemoryStream()
    graph.Bitmap.Save(ms, imageFormat)
    domainTile.Data <- ms.ToArray()
    domainTile

let render iterationsToCheck (tile:TileDetails) (size:System.Drawing.Size) viewPort =     
    let graph = new Graph(size.Width, size.Height, viewPort, iterations)
    graph |> renderSet iterationsToCheck

    let tile = graph |> toDomainTile tile 
    tile |> repository.Save
    tile.Data
    
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
    printfn "%s" tile.Filename

    toRectangleD tile |> render iterations tile size 

let tileNeedToBeRendered (tile:TileDetails) = 
    repository.DoesTileExist(tile.X, tile.Y, tile.Zoom, Tile.MandelbrotSetName) |> not

//
//
//let renderZoomLevel zoom = 
//    let cellCount = zoom |> zoomToCellCount |> int
//    seq{
//        for x in 0 .. (cellCount - 1) do
//            for y in 0 .. (cellCount - 1) do
//                yield (x,y)
//    }
//    |> Seq.map (fun (x,y) -> {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom})
//    |> PSeq.filter (fun tile -> tile |> tileNeedToBeRendered)
//    |> PSeq.iter (fun tile -> renderCell tile)
////    |> Seq.filter (fun tile -> tile |> tileNeedToBeRendered)
////    |> Seq.iter (fun tile -> renderCell tile)
//    

let generateAndSaveTile x y zoom tileSetName = 
    {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom}
    |> renderCell

let getTileImageByte (x, y, zoom, tileSetName) =
    let image = repository.TryGetTileImageByte (x, y, zoom, tileSetName )
    match image with 
    | null -> generateAndSaveTile x y zoom tileSetName
    | _ -> image
