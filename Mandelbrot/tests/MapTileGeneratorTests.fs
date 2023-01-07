module Mandelbrot.MapTileGeneratorTests

open NUnit.Framework
open FsUnit
open Mandelbrot.MandelbrotCalculator
open System.Numerics
open MapTileGenerator

let mandelbrotCalculator = new MandelbrotCalculator()

let iterationsToCheck = 100
    
let size = System.Drawing.Size(512,512) 

let viewPort = {
    XMin = -0.001
    XMax =  0.001
    YMin = -0.001
    YMax =  0.001
}

let testPrint (s:string) = TestContext.Progress.WriteLine(s)

let graph = Graph(size.Width, size.Height, viewPort, iterationsToCheck)

let performance (name : string) (fn: (Unit -> Unit) ) : System.TimeSpan =
    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
    fn() |> ignore
    stopwatch.Stop()
   
    testPrint(sprintf "Duration : %A" (stopwatch.Elapsed))
   
    stopwatch.Elapsed
  
    
[<Test>]
let ``Performance 2``()=    
    
    performance "test1" (fun () -> graph |> mandelbrotCalculator.renderSet iterationsToCheck) 
    |> ignore
    performance "test2" (fun () -> graph |> mandelbrotCalculator.renderSet iterationsToCheck) 
    |> ignore
    performance "test3" (fun () -> graph |> mandelbrotCalculator.renderSet iterationsToCheck) 
    |> ignore
