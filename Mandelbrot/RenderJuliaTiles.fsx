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
open FSharp.Collections.ParallelSeq

let viewPort : RectangleD = {
    XMin = -2.0
    XMax = 2.0
    YMin = -1.5
    YMax = 1.5}

let drawJulia(c: Complex)(index: int) = 
    let graph = new Graph(4096, 4096, viewPort, 100)

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

let start = Complex(-1.0, -1.0)
let increment = Complex(0.01, 0.01)

let x = id

(complexSequence (start) (fun c -> c + increment)) 250
|> Seq.mapi (fun i c -> (i, c))
|> PSeq.iter (fun (i, c) -> 
        drawJulia c i
        printfn "Rendered image(%d): %f + %f" i c.Real c.Imaginary
    )


// #time "on"
