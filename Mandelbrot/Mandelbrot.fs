module Mandelbrot.MandelbrotCalculator

open Mandelbrot
open System.Numerics

let sqr (x:Complex) = x*x

type InSetResult = 
    | NotInSet of iterationsChecked :int
    | InSet
type MandelbrotCalculator() =
    let mandleBrotValuesSequence (value:Complex) = 
        let nextValue previousValue = 
            let nextValue = (previousValue |> sqr) + value
            Some(nextValue, nextValue)
        
        Seq.unfold nextValue value

    let valueOutsideSet (x:Complex) = Complex.Abs(x) > 2.

    let inSetToMagnitude = 
        function 
        | InSetResult.NotInSet(x) -> Some(x)
        | InSetResult.InSet -> None

    let inSetWithResultInternal(iterationsToCheck:int)(v:Complex) = 
        let lastValue = 
            v
            |> mandleBrotValuesSequence
            |> Seq.mapi (fun i v -> (i,v))
            |> Seq.take iterationsToCheck
            |> Seq.takeWhile (fun (_, v) -> valueOutsideSet(v) |> not)
            |> Seq.tryLast

        match lastValue with
        | Some(index,_) when index >= (iterationsToCheck-1) -> InSet
        | None -> NotInSet(0)
        | Some(index,_) -> NotInSet(index)

    let fn iterationsToCheck (value:PointD) = 
        value.ToComplex
        |> (inSetWithResultInternal iterationsToCheck)
        |> inSetToMagnitude

    member this.inSetWithResult = inSetWithResultInternal

    member this.inSet(iterationsToCheck:int)(v:Complex) = 
        v
        |> mandleBrotValuesSequence
        |> Seq.take iterationsToCheck
        |> Seq.exists valueOutsideSet
        |> not

    member this.renderSet iterationsToCheck (graph: Graph) =
        graph.IterateGraph (fn iterationsToCheck)

    
// let mandleBrotValuesSequence (value:Complex) = 
//     let nextValue previousValue = 
//         let nextValue = (previousValue |> sqr) + value
//         Some(nextValue, nextValue)
    
//     Seq.unfold nextValue value

// let valueOutsideSet (x:Complex) = Complex.Abs(x) > 2.

// let inSet (iterationsToCheck:int) (v:Complex) = 
//     v
//     |> mandleBrotValuesSequence
//     |> Seq.take iterationsToCheck
//     |> Seq.exists valueOutsideSet
//     |> not

// let inSetWithResult(iterationsToCheck:int) (v:Complex) = 
//     let lastValue = 
//         v
//         |> mandleBrotValuesSequence
//         |> Seq.mapi (fun i v -> (i,v))
//         |> Seq.take iterationsToCheck
//         |> Seq.takeWhile (fun (_, v) -> valueOutsideSet(v) |> not)
//         |> Seq.tryLast

//     match lastValue with
//     | Some(index,_) when index >= (iterationsToCheck-1) -> InSet
//     | None -> NotInSet(0)
//     | Some(index,_) -> NotInSet(index)


// let inSetToMagnitude = 
//     function 
//     | InSetResult.NotInSet(x) -> Some(x)
//     | InSetResult.InSet -> None

// let fn iterationsToCheck (value:PointD) = 
//     value.ToComplex
//     |> (inSetWithResult iterationsToCheck)
//     |> inSetToMagnitude

// let renderSet iterationsToCheck (graph: Graph) =
//     graph.IterateGraph (fn iterationsToCheck)
