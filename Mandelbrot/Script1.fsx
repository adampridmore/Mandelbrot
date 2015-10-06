#r "System.Drawing.dll"
#r "..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"

#load "PointD.fs"
#load "GraphHelpers.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open System.Numerics
open System.Drawing

let renderSet filename iterationsToCheck = 

    let graph = new Graph.Graph(1024, 768,-2.,2.,-2.,2.)
    graph.DrawAxes()
        
    let sqr x = x*x
    //let fn x y = (System.Math.Sqrt (x*x + y*y)) < 1.
    //let fn (x:float) (y:float) = ((x |> sqr) + ( ( ( (5.*y) / 4.) - Math.Sqrt(Math.Abs(x))) |> sqr) ) < 1.
    let fn x y = new Complex(x,y) |> (MandelbrotCalc.inSet iterationsToCheck)

    graph.IterateGraph fn
        
    graph.Bitmap.Save(filename)

seq{1..10..100}
|> Seq.iter (printfn "%A")
//|> Seq.iter(fun iterationsToCheck -> 
//    let filename = sprintf @"C:\temp\mandlebrot\%d.bmp" iterationsToCheck
//    renderSet filename iterationsToCheck)
