// #r @"./bin/Debug/netcoreapp7.0/FSharp.Collections.ParallelSeq.dll"
// #r @"./bin/Debug/netcoreapp7.0/runtime.osx.10.10-x64.CoreCompat.System.Drawing.dll"
// #r @"./bin/Debug/netcoreapp7.0/System.Drawing.Common.dll" 

// #r @"../Repository/bin/Debug/netcoreapp7.0/Repository.dll"



#r @"./bin/Debug/netcoreapp7.0/FSharp.Collections.ParallelSeq.dll"
#r @"./bin/Debug/netcoreapp7.0/SixLabors.ImageSharp.dll"
#r @"./bin/Debug/netcoreapp7.0/SixLabors.ImageSharp.Drawing.dll"


#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Image2.fs"
#load "Graph.fs"
#load "Calculator.fs"
#load "Julia.fs"
#load "Mandelbrot.fs"
// #load "MapTileGenerator.fs"

open Mandelbrot
open System.Numerics

let viewPort : RectangleD = {
    XMin = -2.0
    XMax = 0.75
    YMin = -1.5
    YMax = 1.5}

let graph = new Graph(512, 512, viewPort, 100)

let drawJulia(c: Complex)(index: int) = 
    // let c = new Complex(-1,0)
    let mandlebrot = new JuliaCalculator(c)

    mandlebrot.renderSet(100)(graph)

    let bitmap  = graph.Bitmap

    let fileName = sprintf "JuliaPics/%06i_render%f-%f.png" index c.Real c.Imaginary
    bitmap.Save(fileName)


let complexSequence (start) (nextValue : Complex -> Complex) number : seq<Complex> = 
    let fn previousValue = 
        let nextValue2 = nextValue(previousValue)
        Some(nextValue2, nextValue2)

    Seq.unfold fn start
    |> Seq.take number

let start = Complex(0.0, 0.0)
let increment = Complex(0.01, 0.0)

(complexSequence (start) (fun c -> c + increment)) 100
|> Seq.iteri (fun i c -> drawJulia c i)

// #time "on"
