open System
open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open FSharp.Collections.ParallelSeq
open Repository.Domain
open MapTileGenerator
open System.Configuration




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


let private repository = Repository.TileRepository("mongodb://localhost/tiles")

let tilesetName = "Mandelbrot" 

let renderZoomLevel zoom = 
    let cellCount = zoom |> zoomToCellCount |> int
    seq{
        for x in 0 .. (cellCount - 1) do
            for y in 0 .. (cellCount - 1) do
                yield (x,y)
    }
    |> Seq.map (fun (x,y) -> {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom})
    // |> PSeq.withDegreeOfParallelism 3
    |> PSeq.map (fun t ->
        let stopwatch = System.Diagnostics.Stopwatch.StartNew()

        let tile = getTileImageByte (t.X, t.Y, t.Zoom, tilesetName, repository)
        (printfn "%s \t Duration: %A)" (t.Filename) (stopwatch.Elapsed) )
        tile
    )
    |> PSeq.iter ignore

    
// #time "on"


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"

    seq{0..30} |> Seq.iter renderZoomLevel
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



    0 // return an integer exit code
