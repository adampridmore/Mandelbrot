namespace Mandelbrot

open Mandelbrot.Calculator
open System.Numerics

type MandelbrotCalculator() =
    inherit BaseCalculator()
    
    override this.sequence(value:Complex) : seq<Complex> = 
        let nextValue previousValue = 
            let nextValue = (previousValue |> sqr) + value
            Some(nextValue, nextValue)
        
        Seq.unfold nextValue value

