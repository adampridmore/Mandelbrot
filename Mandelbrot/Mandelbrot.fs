module MandelbrotCalc
open NUnit.Framework
open FsUnit
open System.Numerics

let sqr (x:Complex) = x*x

let inSet (v:Complex) = 
    let iterationsToCheck = 500

    let fn currentValue = 
        (currentValue |> sqr) + v
    
    Seq.unfold (fun state -> let nextValue = fn state
                             Some(nextValue, nextValue)) v
    |> Seq.take iterationsToCheck
    |> Seq.exists (fun (x:Complex) -> (Complex.Abs(x) > 2.))
    |> not

[<Test>]
let ``outside set``()=
    let v = new Complex(100.,100.)
    let x = v |> inSet
    x |> should equal false

[<Test>]
let ``inside set``()=
    new Complex(0.,0.) |> inSet |> should equal true
