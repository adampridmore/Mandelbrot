module MandelbrotCalc
open System.Numerics

let sqr (x:Complex) = x*x

let defaultIterationsToCheck = 100

type InSetResult = 
    | NotInSet 
    | InSet of value : int

let inSet (iterationsToCheck:int) (v:Complex) = 
    let fn currentValue = 
        (currentValue |> sqr) + v
    
    Seq.unfold (fun state -> let nextValue = fn state
                             Some(nextValue, nextValue)) v
    |> Seq.take iterationsToCheck
    |> Seq.exists(fun (x:Complex) -> (Complex.Abs(x) > 2.))
    |> not

//let inSetWithResult(iterationsToCheck:int) (v:Complex) = 
//    let fn currentValue = 
//        (currentValue |> sqr) + v
//    
//    Seq.unfold (fun state -> let nextValue = fn state
//                             Some(nextValue, nextValue)) v
//    |> Seq.take iterationsToCheck
//    //|> Seq.takeWhile (fun (x:Complex) -> (Complex.Abs(x) > 2.))
//    |> Seq.mapi (fun i x -> i)
//    |> Seq.last
//    |> (fun (i) -> match i with 
//                   | i when i = iterationsToCheck -> NotInSet
//                   | i -> InSet(i))
//
//   