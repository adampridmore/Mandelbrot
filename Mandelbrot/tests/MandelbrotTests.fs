module Mandelbrot.MandelbrotCalculatorTests

open NUnit.Framework
open FsUnit
open Mandelbrot.MandelbrotCalculator
open System.Numerics

[<Test>]
let ``outside set``()=
    Complex(100., 100.)
    |> (inSet 10)
    |> should equal false

[<Test>]
let ``inside set``()=
    Complex.Zero 
    |> (inSet 10) 
    |> should equal true
    
[<Test>]
let ``outside set with result``()=
    Complex(100.,100.)
    |> (inSetWithResult 10)
    |> should equal (InSetResult.NotInSet(0))

[<Test>]
let ``inside set with result``()=
    Complex.Zero 
    |> (inSetWithResult 10) 
    |> should equal (InSetResult.InSet)

