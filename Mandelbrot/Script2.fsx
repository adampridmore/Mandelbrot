#r "System.Drawing.dll"
#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.2.0.0.0\Lib\Net40\FSharp.PowerPack.dll"
#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "GraphHelpers.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open System.Numerics
open System.Drawing
open MandelbrotCalc
open RectangleD
open Microsoft.FSharp.Collections

let iterationsToCheck = 200
//let size = new System.Drawing.Size(3840, 2160)
//let size = new System.Drawing.Size(1080, 720)
//let size = new System.Drawing.Size(1280, 960) // HD
let size = new System.Drawing.Size(1920, 1080) // HD

let render index r = 
    let filename = sprintf @"C:\temp\mandlebrot\b-%dx%d-%d-%d.bmp" size.Width size.Height iterationsToCheck index
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    let graph = new Graph.Graph(size.Width, size.Height, r)
    graph |> renderSet iterationsToCheck
    graph.Bitmap.Save(filename)
    printfn "Duration %d %fs" index stopwatch.Elapsed.TotalSeconds

//System.Diagnostics.Process.Start(filename)

//let transformations = [
//    { XMin = -2.5
//      XMax = 1.0
//      YMin = -1.1
//      YMax = 1.1 };
//    { XMin = -0.7603053435
//      XMax = -0.5404580153
//      YMin = 0.3225806452
//      YMax = 0.5923753666 };
//    { XMin = -0.6528213295;
//      XMax = -0.6338617629;
//      YMin = 0.4628337324;
//      YMax = 0.4825829738;}
//    ]

let transformations = [
    { XMin = -2.5;
      XMax = 1.0;
      YMin = -1.0;
      YMax = 1.0;};
    { XMin = -1.34254386;
      XMax = -1.124561404;
      YMin = -0.2275132275;
      YMax = -0.0873015873;};
    { XMin = -1.246150567;
      XMax = -1.23717931;
      YMin = -0.1744701996;
      YMax = -0.1668661292;};
    { XMin = -1.241978619;
      XMax = -1.241743358;
      YMin = -0.1707687473;
      YMax = -0.1704066487;};
    { XMin = -1.241862324;
      XMax = -1.241807053;
      YMin = -0.1705752449;
      YMax = -0.1704823254;}
    ]

let everyNth n seq =
    seq 
    |> Seq.mapi (fun i v -> (i,v))
    |> Seq.filter (fun (i,_) -> (i % n) = 0)
    |> Seq.map(fun (_,v)-> v)

transformations 
|> Seq.pairwise 
|> Seq.map (fun (a,b) -> translate 100 a b)
|> Seq.concat
//|> (everyNth 50)
|> Seq.iteri render
