module Mandelbrot.JuliaCalculator

open Mandelbrot.Calculator;
open Mandelbrot
open System.Numerics

type JuliaCalculator(c: Complex) =
  inherit Mandelbrot.Calculator.Calculator()

  override this.sequence(value:Complex) = 
    let nextValue previousValue = 
        let nextValue = (previousValue |> sqr) + c
        Some(nextValue, nextValue)
    
    Seq.unfold nextValue value
