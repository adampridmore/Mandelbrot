module Mandelbrot.MandelbrotCalculator

open Mandelbrot
open System.Numerics


type InSetResult =
    | NotInSet of iterationsChecked :int
    | InSet

let inSet (iterationsToCheck:int) (v:Complex) =
    let mutable z = v
    let mutable i = 0
    let mutable inside = true
    while i < iterationsToCheck && inside do
        if z.Real * z.Real + z.Imaginary * z.Imaginary > 4.0 then
            inside <- false
        else
            z <- z * z + v
            i <- i + 1
    inside

let inSetWithResult(iterationsToCheck:int) (v:Complex) =
    let mutable z = v
    let mutable i = 0
    let mutable escaped = false
    while i < iterationsToCheck && not escaped do
        if z.Real * z.Real + z.Imaginary * z.Imaginary > 4.0 then
            escaped <- true
        else
            z <- z * z + v
            i <- i + 1
    if not escaped then InSet
    else NotInSet i

let inSetToMagnitude = 
    function 
    | InSetResult.NotInSet(x) -> Some(x)
    | InSetResult.InSet -> None

let fn iterationsToCheck (value:PointD) = 
    value.ToComplex
    |> (inSetWithResult iterationsToCheck)
    |> inSetToMagnitude

let renderSet iterationsToCheck (graph: Graph) =
    graph.IterateGraph (fn iterationsToCheck)

let renderSetSequential iterationsToCheck (graph: Graph) =
    graph.IterateGraphSequential (fn iterationsToCheck)
