#r "System.Drawing.dll"
#r "..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r "..\packages\FSPowerPack.Core.Community.2.0.0.0\Lib\Net40\FSharp.PowerPack.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "PointD.fs"
#load "GraphHelpers.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open System.Numerics
open System.Drawing
open MandelbrotCalc

let render iterationsToCheck (size:System.Drawing.Size) filename= 
    let graph = new Graph.Graph(size.Width, size.Height,-2.5, 1., -1.1, 1.1)
    graph |> renderSet iterationsToCheck
    graph.Bitmap.Save(filename)

let doRender iterationsToCheck =
    //let size = new System.Drawing.Size(3840, 2160)
    let size = new System.Drawing.Size(1080, 720)
    //let size = new System.Drawing.Size(1280, 960)
    let filename = sprintf @"C:\temp\mandlebrot\%dx%d-%d.bmp" size.Width size.Height iterationsToCheck

    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    filename |> render iterationsToCheck size
    printfn "Duration %fs" stopwatch.Elapsed.TotalSeconds
    //System.Diagnostics.Process.Start(filename)

Seq.singleton 50
//seq{1..5..100}
|> Seq.iter doRender



