#r "nuget: FSharp.Collections.ParallelSeq, 1.1.4"
#r "nuget: runtime.osx.10.10-x64.CoreCompat.System.Drawing"
#r "System.Drawing.Common.dll"

#r @"../Repository/bin/Debug/netcoreapp3.1/Repository.dll"
#r @"./bin/Debug/netcoreapp3.1/Mandelbrot.dll"

#r @"System.Configuration.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"
#load "MapTileGenerator.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections
open FSharp.Collections.ParallelSeq
open Repository.Domain
open MapTileGenerator
open System.Configuration

    
#time "on"

// let filename = "Mandelbrot_1.png"

// let zoom = 1
// let tile = {X=0;Y=0;Filename=("tile1.png");Zoom=zoom}
// let imageBytes = getTileImageByte (tile.X, tile.Y, tile.Zoom, tilesetName, repository)
// System.IO.File.WriteAllBytes(path, imageBytes)

// let iterations = 400
let iterations = 10000
// let imageResolution = (2.0**12.0) |> int32 // 4096
// let imageResolution = 1024
// let imageResolution = 4096 * 2
let imageResolution = 1920
// let imageResolution = 1280

let renderImage index span = 
    let real = -1.63149737002
    let imaginary = 1.985e-8
    
    // let span = 1.0 / zoom
    let filename = sprintf "Images/Mandelbrot_%d.png" index

    let ratio = 16.0 / 9.0
    let viewPort = {
        RectangleD.XMin = real - span
        XMax = real + span
        YMin = imaginary - ((span * ratio) / 2.0)
        YMax = imaginary + ((span * ratio) / 2.0)
    }

    let size = System.Drawing.Size(imageResolution,((imageResolution |> float) * 0.8) |> int32)

    let graph = Graph(size.Width, size.Height, viewPort, iterations)

    let stopwatch = System.Diagnostics.Stopwatch.StartNew()

    graph |> renderSet iterations

    graph.Bitmap.Save(filename)


// let logSequence = Seq.map(fun x -> printfn "%A" x; x)

// let zoomFactor = 0.8
// let zoomFactor = 0.75

let targetZoom = 0.75 ** 25.0
let steps = 100
let zoomFactor = System.Math.Pow(targetZoom, 1.0 / (steps |> float ))


seq{1..steps}
|> Seq.map(fun i -> i, zoomFactor ** (i |> float))
|> Seq.map(fun x -> printfn "%A" x; x)
// |> Seq.iter (ignore)
|> Seq.iter (fun (i, span) -> renderImage i span)
// |> Seq.iter (printf "%f\n")

