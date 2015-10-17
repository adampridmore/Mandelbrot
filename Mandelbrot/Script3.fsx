#load "RectangleD.fs"

open Mandelbrot

let fromR = { XMin = -100.
              XMax = 100.
              YMin = -100.
              YMax = 100. }

let toR = { XMin = 0.
            XMax = 20.
            YMin = 0.
            YMax = 20. }

let iterations = 2

//RectangleD.TranslationSeq3 iterations fromR toR
//|> Seq.iter (printfn "%A")


let centerX1 = -10 
let centerX2 = 10

let nextCenterX x1 x2 totalIterations currentIteration = 
    x1 + (((x2 - x1) / totalIterations) * currentIteration)

nextCenterX centerX1 centerX2 10 2

