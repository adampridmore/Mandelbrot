module MandelbrotCalc
open System.Numerics

let sqr (x:Complex) = x*x

let defaultIterationsToCheck = 100

type InSetResult = 
    | NotInSet of value :int
    | InSet

let mandleBrotValuesSequence (v:Complex) = 
    let fn currentValue = 
        (currentValue |> sqr) + v
    
    Seq.unfold (fun state -> let nextValue = fn state
                             Some(nextValue, nextValue)) v

let valueOutsideSet (x:Complex) = Complex.Abs(x) > 2.

let inSet (iterationsToCheck:int) (v:Complex) = 
    v
    |> mandleBrotValuesSequence
    |> Seq.take iterationsToCheck
    |> Seq.exists valueOutsideSet
    |> not

let inSetWithResult(iterationsToCheck:int) (v:Complex) = 
    let lastValue = v
                    |> mandleBrotValuesSequence
                    |> Seq.mapi (fun i v -> (i,v))
                    |> Seq.take iterationsToCheck
                    |> Seq.takeWhile (fun (_, v) -> valueOutsideSet(v) |> not)
                    |> Seq.tryLast

    match lastValue with
    | Some(index,_) when index >= (iterationsToCheck-1) -> InSet
    | None -> NotInSet(0)
    | Some(index,_) -> NotInSet(index)


let inSetToMagnitude inSet =
             match inSet with
             | InSetResult.NotInSet(x) -> Some(x)
             | InSetResult.InSet -> None

let fn x y = 
    let inSet = new Complex(x,y) 
                |> (inSetWithResult 500)
    inSet |> inSetToMagnitude