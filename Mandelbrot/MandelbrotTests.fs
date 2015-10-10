module MandelbrotTests
open NUnit.Framework
open FsUnit
open MandelbrotCalc

[<Test>]
let ``outside set``()=
    createComplex 100. 100.
    |> (inSet 10)
    |> should equal false

[<Test>]
let ``inside set``()=
    createComplex 0. 0. 
    |> (inSet 10) 
    |> should equal true

[<Test>]
let ``outside set with result``()=
    createComplex 100. 100.
    |> (inSetWithResult 10)
    |> should equal (InSetResult.NotInSet(0))

[<Test>]
let ``inside set with result``()=
    createComplex 0. 0. 
    |> (inSetWithResult 10) 
    |> should equal (InSetResult.InSet)

