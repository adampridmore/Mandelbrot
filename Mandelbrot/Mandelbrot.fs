module Mandelbrot.MandelbrotCalculator

open Mandelbrot.Calculator
open Mandelbrot
open System.Numerics

type MandelbrotCalculator() =
    inherit Mandelbrot.Calculator.Calculator()
    
    override this.sequence(value:Complex) : seq<Complex> = 
        let nextValue previousValue = 
            let nextValue = (previousValue |> sqr) + value
            Some(nextValue, nextValue)
        
        Seq.unfold nextValue value

