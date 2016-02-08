#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.2.0.0.0\Lib\Net40\FSharp.PowerPack.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"
#load "Render.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator

let width = 80
let height = 40

let newLine = System.Environment.NewLine

let createRowText() = new System.Text.StringBuilder(new System.String(' ', width))

let grid = 
    seq{0..height-1}
    |> Seq.map (fun _ -> createRowText())
    |> Seq.toArray

grid
|> Seq.map (fun sb -> sb.ToString())
|> Seq.iter (printfn "%s")

let setPixel (x:int) (y:int) = 
    grid.[y].Chars(x) <-'X' 

Render.renderSet width height setPixel