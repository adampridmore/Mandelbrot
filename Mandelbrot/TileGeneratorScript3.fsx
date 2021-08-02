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

let repository = new Repository.TileRepository("mongodb://localhost/tiles")

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

let zoom = 1
let tile = {X=0;Y=0;Filename=("tile1.png");Zoom=zoom}
getTileImageByte (tile.X, tile.Y, tile.Zoom, tilesetName, repository)



// seq{0..30} |> Seq.iter renderZoomLevel
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
