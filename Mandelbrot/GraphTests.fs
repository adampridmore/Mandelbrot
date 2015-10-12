module Mandelbrot.GraphTests
open NUnit.Framework
open FsUnit
open Mandelbrot

[<Test>]
let test() = 
    let view = {RectangleD.XMin = 3. ; XMax = 2.; YMin = 0. ; YMax = 1.}
    let g = new Graph(3,2,view)

    g.IterateGraph (fun p -> (printfn "%A" p) ; Some(1))
