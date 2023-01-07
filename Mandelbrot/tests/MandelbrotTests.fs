module Mandelbrot.MandelbrotCalculatorTests

open NUnit.Framework
open FsUnit
open Mandelbrot.MandelbrotCalculator
open System.Numerics

let mandelbrotCalculator = new MandelbrotCalculator()

[<Test>]
let ``outside set``()=
    Complex(100., 100.)
    |> (mandelbrotCalculator.inSet 10)
    |> should equal false

[<Test>]
let ``inside set``()=
    Complex.Zero 
    |> (mandelbrotCalculator.inSet 10) 
    |> should equal true
    
[<Test>]
let ``outside set with result``()=
    Complex(100.,100.)
    |> (mandelbrotCalculator.inSetWithResult 10)
    |> should equal (InSetResult.NotInSet(0))

[<Test>]
let ``inside set with result``()=
    Complex.Zero 
    |> (mandelbrotCalculator.inSetWithResult 10) 
    |> should equal (InSetResult.InSet)

