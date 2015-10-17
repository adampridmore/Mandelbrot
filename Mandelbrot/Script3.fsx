#r @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#r @"..\packages\FSPowerPack.Core.Community.2.0.0.0\Lib\Net40\FSharp.PowerPack.dll"

#load "Pixel.fs"
#load "ColorModule.fs"
#load "RectangleD.fs"
#load "PointD.fs"
#load "Graph.fs"
#load "Mandelbrot.fs"

open Mandelbrot
open Mandelbrot.MandelbrotCalculator

let width = 80
let height = 40
let minValue = Complex.Create(-2., -1.);
let maxValue = Complex.Create(1., 1.);

let newLine = System.Environment.NewLine

let mapPixelToComplex x y =
    let percentR = (float x) / (float width)
    let percentI = (float y) / (float height)
    let r = minValue.RealPart + (maxValue.RealPart - minValue.RealPart) * percentR
    let i = minValue.ImaginaryPart + (maxValue.ImaginaryPart - minValue.ImaginaryPart) * percentI
    Complex.Create(r,i)

let toChar v = match v with
               | true -> "X"
               | false -> " "

let inSet c = MandelbrotCalculator.inSet 10 c

let renderRow y =
    seq{0..width-1}
    |> Seq.map (fun x -> mapPixelToComplex x y)
    |> Seq.map inSet
    |> Seq.map toChar
    |> Seq.reduce (+)

seq{0..height-1}
|> Seq.map renderRow
|> Seq.map (fun row -> sprintf "%s%s" row newLine)
|> Seq.reduce (+)
|> printfn "%s"