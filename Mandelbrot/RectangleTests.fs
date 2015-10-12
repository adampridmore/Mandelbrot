module Mandelbrot.RectangleTests 

open NUnit.Framework
open FsUnit
open Mandelbrot

[<Test>] 
let toStringTest() = 
    let r = {RectangleD.XMin = 0. ; XMax = 0. ; YMin = 0. ; YMax = 0.}

    printfn "%s" (r.ToString())

[<Test>] 
let t() = 
    let rFrom = {RectangleD.XMin = 0. ; XMax = 0. ; YMin = 0. ; YMax = 0.}
    let rHalf = {RectangleD.XMin = 0. ; XMax = 5. ; YMin = 0. ; YMax = 50.}
    let rTo = {RectangleD.XMin = 0. ; XMax = 10. ; YMin = 0. ; YMax = 100.}

    let steps = RectangleD.TranslationSeq 2 rFrom rTo

    steps |> Seq.iter (printfn "%A")
    
    steps
    |> Seq.length
    |> should equal 2

    steps |> should equal [|rFrom;rHalf|]


[<Test>] 
let t2() = 
    let rFrom = {RectangleD.XMin = 100. ; XMax = 200. ; YMin = 1000. ; YMax = 2000.}
    let rTo = {RectangleD.XMin = 140. ; XMax = 160. ; YMin = 1200. ; YMax = 1800.}

    let steps = RectangleD.TranslationSeq 50 rFrom rTo

    steps |> Seq.iter (printfn "%A")
