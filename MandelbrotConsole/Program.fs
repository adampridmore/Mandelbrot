module MandelbrotConsole.Program

open System
open System.Threading.Tasks
open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Repository.Domain
open MapTileGenerator
open System.Configuration
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.PixelFormats

let drawImage() =

    use image = new Image<Rgba32>(10,10)

    image[5,5] <- Rgba32(100uy,255uy,255uy)

    let encoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder()
    image.Save("image.bmp",encoder)

    printfn "Pixel 1: %O" image[0,0]
    printfn "Pixel 2: %O" image[5,5]

    // DrawImageExtensions.DrawImage(this IImageProcessingContext source, Image image, GraphicsOptions options)


    // image.Mutate(x => x.Fill(Rgba32.HotPink))

    ()


// #r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
// #r @"..\packages\FSPowerPack.Core.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.dll"
// #r @"..\Repository\bin\Debug\Repository.dll"
// #r @"System.Configuration.dll"

// #load "Pixel.fs"
// #load "ColorModule.fs"
// #load "RectangleD.fs"
// #load "PointD.fs"
// #load "Graph.fs"
// #load "Mandelbrot.fs"
// #load "MapTileGenerator.fs"


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
    let options = ParallelOptions(MaxDegreeOfParallelism = Environment.ProcessorCount)
    Parallel.ForEach(tiles, options, fun (x, y) ->
        getTileImageByteSequential (x, y, zoom, tilesetName, repository) |> ignore
        printfn "%s" (toFilename x y zoom)
    ) |> ignore

    
// #time "on"


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
    //renderZoomLevel 0
    //renderZoomLevel 1
    //renderZoomLevel 2
    //renderZoomLevel 3
    //renderZoomLevel 4
    //renderZoomLevel 5
    //renderZoomLevel 6
    //renderZoomLevel 7
    //renderZoomLevel 8
    //renderZoomLevel 9
    //renderZoomLevel 10
    //renderZoomLevel 11


    // drawImage()

    0 // return an integer exit code

