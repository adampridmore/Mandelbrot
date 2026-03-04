module MandelbrotConsole.Program

open System
open System.Threading.Tasks
open Mandelbrot
open MapTileGenerator
open MandelbrotGpu

let private connectionString =
    match Environment.GetEnvironmentVariable("ConnectionStrings__MongoDb") with
    | null | "" -> "mongodb://localhost/tiles"
    | cs -> cs

let private repository = Repository.TileRepository(connectionString)

let tilesetName = "Mandelbrot"

let renderZoomLevel zoom =
    let cellCount = zoom |> zoomToCellCount |> int
    let tiles = [| for x in 0 .. (cellCount - 1) do
                       for y in 0 .. (cellCount - 1) do
                           yield (x, y) |]
    let options = ParallelOptions(MaxDegreeOfParallelism = if GpuRenderer.IsAvailable then 1 else Environment.ProcessorCount)
    Parallel.ForEach(tiles, options, fun (x, y) ->
        getTileImageByteSequential (x, y, zoom, tilesetName, repository) |> ignore
        printfn "%s" (toFilename x y zoom)
    ) |> ignore

[<EntryPoint>]
let main argv =
    let maxZoom =
        if argv.Length > 0 then
            match System.Int32.TryParse(argv[0]) with
            | true, n -> n
            | _ ->
                eprintfn "Invalid argument '%s', using default max zoom 30" argv[0]
                30
        else 30

    seq{0..maxZoom} |> Seq.iter renderZoomLevel
    0
