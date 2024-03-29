﻿// #r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"./bin/Debug/netcoreapp7.0/FSharp.Collections.ParallelSeq.dll"

#r @"./bin/Debug/netcoreapp7.0/SixLabors.ImageSharp.dll"
#r @"./bin/Debug/netcoreapp7.0/SixLabors.ImageSharp.Drawing.dll"

// #r @"..\packages\FSPowerPack.Core.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.dll"
#r @"../Repository/bin/Debug/netcoreapp7.0/Repository.dll"
#r @"System.Configuration.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Image2.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"
#load "MapTileGenerator.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator
open Microsoft.FSharp.Collections
open Repository.Domain
open MapTileGenerator
open System.Configuration
open FSharp.Collections.ParallelSeq

let private repository = new Repository.TileRepository("mongodb://localhost/tiles")

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
    |> PSeq.map (fun tile -> (printfn "%s" tile.Filename) ; getTileImageByte (tile.X, tile.Y, tile.Zoom, tilesetName, repository) )
    |> PSeq.iter ignore

    
#time "on"

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
