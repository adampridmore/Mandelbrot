#r @"./bin/Debug/netcoreapp7.0/FSharp.Collections.ParallelSeq.dll"
#r @"./bin/Debug/netcoreapp7.0/runtime.osx.10.10-x64.CoreCompat.System.Drawing.dll"
// #r @"./bin/Debug/netcoreapp7.0/System.Drawing.Common.dll"

#r @"../Repository/bin/Debug/netcoreapp7.0/Repository.dll"

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
open MapTileGenerator
open System.Configuration


let viewPort : RectangleD = {
    XMin = -1.0
    XMax = -1.0
    YMin = -1.0
    YMax = 1.0}

let graph = new Graph(100, 100, viewPort, 100)

let mandlebrot = MandelbrotCalculator.MandelbrotCalculator()

mandlebrot.renderSet(100)(graph)

let bitmap : System.Drawing.Bitmap = graph.Bitmap

#time "on"
