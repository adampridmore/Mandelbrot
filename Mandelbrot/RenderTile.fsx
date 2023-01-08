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
#load "Julia.fs"
#load "Mandelbrot.fs"
// #load "MapTileGenerator.fs"

open Mandelbrot
// open Mandelbrot.MandelbrotCalculator
open Mandelbrot.JuliaCalculator
open System.Numerics
open Microsoft.FSharp.Collections

let viewPort : RectangleD = {
    XMin = -2.0
    XMax = 0.75
    YMin = -1.5
    YMax = 1.5}

let graph = new Graph(512, 512, viewPort, 100)

let c = new Complex(0,0)
let mandlebrot = Mandelbrot.JuliaCalculator.JuliaCalculator(c)

mandlebrot.renderSet(100)(graph)

let bitmap  = graph.Bitmap

let fileName = sprintf "JuliaPics/render%f-%f.png" c.Real c.Imaginary
bitmap.Save(fileName)


#time "on"
