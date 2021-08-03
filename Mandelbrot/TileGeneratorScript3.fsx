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

let filename = "Mandelbrot_1.png"

// let zoom = 1
// let tile = {X=0;Y=0;Filename=("tile1.png");Zoom=zoom}
// let imageBytes = getTileImageByte (tile.X, tile.Y, tile.Zoom, tilesetName, repository)
// System.IO.File.WriteAllBytes(path, imageBytes)

let real = -1.63149737002
let imaginary = 1.985e-8
let zoom = 1.0 / 1.0

let viewPort = { 
    RectangleD.XMin = -1.0
    XMax = 1.0
    YMin = -1.0
    YMax = 1.0
}
let size = new System.Drawing.Size(100,100)
let iterations = 400

let graph = Graph(size.Width, size.Height, viewPort, iterations)

let stopwatch = System.Diagnostics.Stopwatch.StartNew()

graph |> renderSet iterations

graph.Bitmap.Save(filename)
