module MapTileGenerator

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections
open Repository.Domain
open Repository

let private tileSize = 256
let private iterations = 400

let imageFormat = System.Drawing.Imaging.ImageFormat.Png

type TileDetails = {
        X: int;
        Y: int;
        Zoom: int;
        Filename: string
    }

let private toDomainTile (tile:TileDetails) (duration: System.TimeSpan) (graph:Graph) = 
    let domainTile = Tile.CreateTile(tile.X, tile.Y, tile.Zoom, iterations, Tile.MandelbrotSetName, duration)
    
    domainTile.Id <- tile.Filename
    let ms = new System.IO.MemoryStream()
    graph.Bitmap.Save(ms, imageFormat)
    domainTile.Data <- ms.ToArray()
    domainTile

let private render iterationsToCheck (tileDetails:TileDetails) (size:System.Drawing.Size) (viewPort: RectangleD) (repository:TileRepository) : Tile =
    let graph = Graph(size.Width, size.Height, viewPort, iterations)

    let stopwatch = System.Diagnostics.Stopwatch.StartNew()

    graph |> renderSet iterationsToCheck

    stopwatch.Stop()

    let tile = graph |> toDomainTile tileDetails (stopwatch.Elapsed)
    tile |> repository.Save
    tile
    
let private size = System.Drawing.Size(tileSize, tileSize)

let private fullViewport = {XMin = -2.0; XMax = 2.0; YMin = -2.; YMax = 2.}

let zoomToCellCount zoom =
    (2. ** (zoom |> float)) |> float

let private toRectangleD (tile:TileDetails) =
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

let private renderCell (tileDetails:TileDetails) (repository: TileRepository) : Tile = 
    let rectangle = toRectangleD tileDetails
    render iterations tileDetails size rectangle repository

let private tileNeedToBeRendered (tileDetails:TileDetails) (repository:TileRepository) = 
    repository.DoesTileExist(tileDetails.X, tileDetails.Y, tileDetails.Zoom, Tile.MandelbrotSetName) |> not

let private generateAndSaveTile x y zoom tileSetName = 
    {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom}
    |> renderCell

let getTileImageByte (x, y, zoom, tileSetName, (repository:TileRepository)) : byte[] =
    let image = repository.TryGetTileImageByte (x, y, zoom, tileSetName )
    match image with 
    | null -> (generateAndSaveTile x y zoom tileSetName repository).Data
    | _ -> image
