module MapTileGenerator

open Mandelbrot
open Repository.Domain
open Repository

let private tileSize = 256
let private iterations = 400

// let imageFormat = System.Drawing.Imaging.ImageFormat.Png

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
    graph.Bitmap.Save(ms)
    domainTile.Data <- ms.ToArray()
    domainTile

let private render iterationsToCheck (tile:TileDetails) (size:System.Drawing.Size) viewPort (repository:TileRepository) : Tile =
    let graph = Graph(size.Width, size.Height, viewPort, iterations)

    let stopwatch = System.Diagnostics.Stopwatch.StartNew()

    let mandlebrot = new MandelbrotCalculator()

    graph |> mandlebrot.renderSet iterationsToCheck

    stopwatch.Stop()

    let tile = graph |> toDomainTile tile (stopwatch.Elapsed)
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
    
let toFilename x y zoom = sprintf @"tile_zm%d_x%d_y%d.%s" zoom x y (Mandelbrot.Image2.imageTypeExtension)

let private renderCell (tile:TileDetails) (repository: TileRepository) = 
    let rectangle = toRectangleD tile
    render iterations tile size rectangle repository

let private tileNeedToBeRendered (tile:TileDetails) (repository:TileRepository) = 
    repository.DoesTileExist(tile.X, tile.Y, tile.Zoom, Tile.MandelbrotSetName) |> not

let private generateAndSaveTile x y zoom tileSetName = 
    {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom}
    |> renderCell

let getTileImageByte (x, y, zoom, tileSetName, (repository:TileRepository)) : byte[] =
    let image = repository.TryGetTileImageByte (x, y, zoom, tileSetName )
    match image with 
    | null -> (generateAndSaveTile x y zoom tileSetName repository).Data
    | _ -> image
