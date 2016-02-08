module Render

open Mandelbrot
open Mandelbrot.MandelbrotCalculator

let minValue = Complex.Create(-2., -1.);
let maxValue = Complex.Create(1., 1.);

let inSet c = MandelbrotCalculator.inSet 10 c

let renderSet width height setPixelInSet = 
    let mapPixelToComplex x y =
        let percentR = (float x) / (float width)
        let percentI = (float y) / (float height)
        let r = minValue.RealPart + (maxValue.RealPart - minValue.RealPart) * percentR
        let i = minValue.ImaginaryPart + (maxValue.ImaginaryPart - minValue.ImaginaryPart) * percentI
        Complex.Create(r,i)

    let renderRow y =
        seq{0..width-1}
        |> Seq.map (fun x -> (x,y,mapPixelToComplex x y))
        |> Seq.map (fun (x,y,c) -> (x,y, c |> inSet))
        |> Seq.filter (fun (_,_,inSet) -> inSet )
        |> Seq.iter (fun  (x,y, _) -> setPixelInSet x y)

    seq{0..(height-1)}
    |> Seq.iter renderRow
