module MapTileGenerator

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Mandelbrot.Image2
open Microsoft.FSharp.Collections
open Repository.Domain
open Repository
open MandelbrotGpu

let private tileSize = 256

let iterationsForZoom zoom = max 100 (zoom * 50)

// let imageFormat = System.Drawing.Imaging.ImageFormat.Png

type TileDetails = {
        X: int;
        Y: int;
        Zoom: int;
        Filename: string
    }

let private toDomainTile (tile:TileDetails) (duration: System.TimeSpan) (graph:Graph) =
    let domainTile = Tile.CreateTile(tile.X, tile.Y, tile.Zoom, iterationsForZoom tile.Zoom, Tile.MandelbrotSetName, duration)
    
    domainTile.Id <- tile.Filename
    let ms = new System.IO.MemoryStream()
    graph.Bitmap.Save(ms)
    domainTile.Data <- ms.ToArray()
    domainTile

let private render iterationsToCheck (tile:TileDetails) (size:System.Drawing.Size) viewPort (repository:TileRepository) : Tile =
    let graph = Graph(size.Width, size.Height, viewPort, iterationsToCheck)

    let stopwatch = System.Diagnostics.Stopwatch.StartNew()

    if GpuRenderer.IsAvailable then
        let results = GpuRenderer.RenderTile(size.Width, size.Height, viewPort.XMin, viewPort.XMax, viewPort.YMin, viewPort.YMax, iterationsToCheck)
        graph.IterateGraphWithIterations(results)
    else
        graph |> renderSet iterationsToCheck

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
    render (iterationsForZoom tile.Zoom) tile size rectangle repository

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

let private renderCellSequential (tile:TileDetails) (repository: TileRepository) =
    let rectangle = toRectangleD tile
    let tileIterations = iterationsForZoom tile.Zoom
    let graph = Graph(size.Width, size.Height, rectangle, tileIterations)
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    if GpuRenderer.IsAvailable then
        let results = GpuRenderer.RenderTile(size.Width, size.Height, rectangle.XMin, rectangle.XMax, rectangle.YMin, rectangle.YMax, tileIterations)
        graph.IterateGraphWithIterations(results)
    else
        graph |> renderSetSequential tileIterations
    stopwatch.Stop()
    let t = graph |> toDomainTile tile stopwatch.Elapsed
    t |> repository.Save
    t

let private generateAndSaveTileSequential x y zoom =
    {X=x; Y=y; Filename=(toFilename x y zoom); Zoom=zoom}
    |> renderCellSequential

let getTileImageByteSequential (x, y, zoom, tileSetName, (repository:TileRepository)) : byte[] =
    let image = repository.TryGetTileImageByte(x, y, zoom, tileSetName)
    match image with
    | null -> (generateAndSaveTileSequential x y zoom repository).Data
    | _ -> image

let private renderAsync (tile:TileDetails) (repository:TileRepository) =
    task {
        let tileIterations = iterationsForZoom tile.Zoom
        let viewPort = toRectangleD tile
        let graph = Graph(tileSize, tileSize, viewPort, tileIterations)
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()
        if GpuRenderer.IsAvailable then
            let results = GpuRenderer.RenderTile(tileSize, tileSize, viewPort.XMin, viewPort.XMax, viewPort.YMin, viewPort.YMax, tileIterations)
            graph.IterateGraphWithIterations(results)
        else
            graph |> renderSet tileIterations
        stopwatch.Stop()
        let domainTile = graph |> toDomainTile tile stopwatch.Elapsed
        do! repository.SaveAsync(domainTile)
        return domainTile
    }

let getTileImageByteAsync (x, y, zoom, tileSetName, (repository:TileRepository)) =
    task {
        let! image = repository.TryGetTileImageByteAsync(x, y, zoom, tileSetName)
        if isNull image then
            let! tile = renderAsync {X=x; Y=y; Filename=(toFilename x y zoom); Zoom=zoom} repository
            return tile.Data
        else
            return image
    }
