module Mandelbrot.GraphTests
open NUnit.Framework
open FsUnit
open Mandelbrot

[<Test>]
let test() = 
    let iterations = 100 
    let view = {RectangleD.XMin = 3. ; XMax = 2.; YMin = 0. ; YMax = 1.}
    let g = Graph(3,2,view, iterations)

    g.IterateGraph (fun p -> (printfn "%A" p) ; Some(1))
   
[<Test>]
let ``simple mapping``() = 
    let minX = 100.
    let maxX = 200.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    Graph.MapValueToPixel minX maxX pixelWidth valueToMap |> should equal 500.

[<Test>]
let ``simple mapping 2``() = 
    let minX = 100.
    let maxX = 500.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    Graph.MapValueToPixel minX maxX pixelWidth valueToMap |> should equal 125.

[<Test>]
let ``simple mapping 3``() = 
    let minX = -100.
    let maxX = 100.
    let pixelWidth = 1000.

    let valueToMap = 0.
    
    Graph.MapValueToPixel minX maxX pixelWidth valueToMap |> should equal 500.

[<Test>]
let ``simple mapping 4``() = 
    let minX = 0.
    let maxX = 10.
    let pixelWidth = 10.

    Graph.MapValueToPixel minX maxX pixelWidth 0. |> should equal 0.
    Graph.MapValueToPixel minX maxX pixelWidth 10. |> should equal 10.

[<Test>]
let ``map pixel to value``()=
    Graph.MapPixelToValue -100. 200. 900 600 |> should equal 100