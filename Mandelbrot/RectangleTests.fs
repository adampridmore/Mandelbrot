module Mandelbrot.RectangleTests 

open NUnit.Framework
open FsUnit
open Mandelbrot

[<Test>] 
let toStringTest() = 
    let r = {RectangleD.XMin = 0. ; XMax = 0. ; YMin = 0. ; YMax = 0.}

    printfn "%s" (r.ToString())

[<Test>]
let ``area``() = 
    let r = {RectangleD.XMin = 100. ; XMax = 120. ; YMin = 1000. ; YMax = 1010.}
    r.Area |> should equal 200.

[<Test>]
let ``aspect ratio``() = 
    let r = {RectangleD.XMin = 100. ; XMax = 120. ; YMin = 1000. ; YMax = 1010.}
    r.AspectRatio |> should equal 0.5

[<Test>]
let ``center``() = 
    let r = {RectangleD.XMin = 100. ; XMax = 120. ; YMin = 1000. ; YMax = 1010.}
    r.CenterX |> should equal 110.
    r.CenterY |> should equal 1005.

[<Test>]
[<Ignore>] 
let ``simple translations``() = 
    let rFrom = {RectangleD.XMin = 0. ; XMax = 10. ; YMin = 0. ; YMax = 10.}
    let rHalf = {RectangleD.XMin = 0. ; XMax = 16. ; YMin = 0. ; YMax = 60.}
    let rTo = {RectangleD.XMin = 0. ; XMax = 20. ; YMin = 0. ; YMax = 100.}

    let steps = RectangleD.TranslationSeq 2 rFrom rTo

    steps |> Seq.iter (printfn "%A")
    
    steps
    |> Seq.length
    |> should equal 2

    steps |> should equal [|rFrom;rHalf|]

    
[<Test>] 
let ``linear zoom translations``() = 
    let rFrom = {RectangleD.XMin = 0. ; XMax = 200. ; YMin = 1100. ; YMax = 1500.}
    //let rHalf = {RectangleD.XMin = 0. ; XMax = 5. ; YMin = 0. ; YMax = 50.}
    let rTo = {RectangleD.XMin = 50. ; XMax = 150. ; YMin = 1200. ; YMax = 1400.}

    let steps = RectangleD.TranslationSeq 2 rFrom rTo

    steps |> Seq.iter (printfn "%A")
    
    steps
    |> Seq.length
    |> should equal 2

    //steps |> should equal [|rFrom;rHalf|]


[<Test>] 
let ``linear zoom translations 3``() = 
    let rFrom = {RectangleD.XMin = 0. ; XMax = 200. ; YMin = 1100. ; YMax = 1500.}
    let rTo = {RectangleD.XMin = 50. ; XMax = 150. ; YMin = 1200. ; YMax = 1400.}

    let steps = RectangleD.TranslationSeq3 2 rFrom rTo
    steps |> Seq.iter (printfn "%A")

[<Test>] 
let ``linear zoom translations 3 II``() = 
    let rFrom = { XMin = -2.5
                  XMax = 1.0
                  YMin = -1.0
                  YMax = 1.0}
    let rTo =   { XMin = 0.3541935484
                  XMax = 0.4761290323
                  YMin = -0.3874239351
                  YMax = -0.2900608519}

    let steps = RectangleD.TranslationSeq3 2 rFrom rTo
    steps |> Seq.iter (printfn "%A")

[<Test>] 
let ``show more complex translations``() = 

    let rFrom = { XMin = -100.
                  XMax = 100.
                  YMin = -100.
                  YMax = 100. }

    let rTo  = { XMin = 0.
                 XMax = 20.
                 YMin = 0.
                 YMax = 20. }

    let steps = RectangleD.TranslationSeq 2 rFrom rTo

    let printRectangle (r:RectangleD) =
        printfn "%f,%f,%f,%f" r.XMin r.YMin r.XMax r.YMax

    steps 
    |> Seq.iter printRectangle
