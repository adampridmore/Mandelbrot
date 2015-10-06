module MandelbrotTests
open NUnit.Framework
open FsUnit
open MandelbrotCalc
open System.Numerics

[<Test>]
let ``outside set``()=
    new Complex(100.,100.)
    |> (inSet defaultIterationsToCheck)
    |> should equal false

[<Test>]
let ``inside set``()=
    new Complex(0.,0.) 
    |> (inSet defaultIterationsToCheck) 
    |> should equal true

[<Test>]
let ``outside set with result``()=
    let value = (new Complex(100.,100.)
                |> (inSetWithResult defaultIterationsToCheck))
    value |> should equal (InSetResult.NotInSet(0))

[<Test>]
let ``inside set with result``()=
    let value = new Complex(0.,0.) 
                |> (inSetWithResult defaultIterationsToCheck) 

    value |> should equal (InSetResult.InSet)

