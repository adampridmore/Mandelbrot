#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.dll"
#r @"..\PaddingtonRepository\bin\Debug\PaddingtonRepository.dll"
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
open PaddingtonRepository.Domain
open MapTileGenerator
open System.Configuration

let private repository = new PaddingtonRepository.TileRepository("mongodb://localhost/tiles")

let tilesetName = "Mandelbrot" 

let renderZoomLevel zoom = 
    let cellCount = zoom |> zoomToCellCount |> int
    seq{
        for x in 0 .. (cellCount - 1) do
            for y in 0 .. (cellCount - 1) do
                yield (x,y)
    }
    |> Seq.map (fun (x,y) -> {X=x;Y=y;Filename=(toFilename x y zoom);Zoom=zoom})
    |> Seq.map (fun tile -> (printfn "%s" tile.Filename) ; getTileImageByte (tile.X, tile.Y, tile.Zoom, tilesetName, repository) )
    |> Seq.iter ignore

    
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
