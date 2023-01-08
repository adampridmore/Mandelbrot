namespace Mandelbrot

open Mandelbrot.Calculator
open System.Numerics

type JuliaCalculator(c: Complex) =
  inherit BaseCalculator()

  override this.sequence(value:Complex) = 
    let nextValue previousValue = 
        let nextValue = (previousValue |> sqr) + c
        Some(nextValue, nextValue)
    
    Seq.unfold nextValue value
