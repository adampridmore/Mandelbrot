module Mandelbrot.Calculator

open Mandelbrot
open System.Numerics

let sqr (x:Complex) = x*x
type InSetResult = 
    | NotInSet of iterationsChecked :int
    | InSet

[<AbstractClass>]
type Calculator() = 
  let valueOutsideSet (x:Complex) = Complex.Abs(x) > 2.

  let inSetToMagnitude = 
      function 
      | InSetResult.NotInSet(x) -> Some(x)
      | InSetResult.InSet -> None

  abstract member sequence : Complex -> seq<Complex>
  
  member this.inSet(iterationsToCheck:int)(v:Complex) = 
      v
      |> this.sequence
      |> Seq.take iterationsToCheck
      |> Seq.exists valueOutsideSet
      |> not

  member this.inSetWithResult(iterationsToCheck:int)(v:Complex) = 
      let lastValue = 
          v
          |> this.sequence
          |> Seq.mapi (fun i v -> (i,v))
          |> Seq.take iterationsToCheck
          |> Seq.takeWhile (fun (_, v) -> valueOutsideSet(v) |> not)
          |> Seq.tryLast

      match lastValue with
      | Some(index,_) when index >= (iterationsToCheck-1) -> InSet
      | None -> NotInSet(0)
      | Some(index,_) -> NotInSet(index)

  member private this.fn iterationsToCheck (value:PointD) = 
      value.ToComplex
      |> (this.inSetWithResult iterationsToCheck)
      |> inSetToMagnitude
  
  member this.renderSet iterationsToCheck (graph: Graph) : Unit =
      graph.IterateGraph (this.fn iterationsToCheck)

