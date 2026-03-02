module Mandelbrot.MapTileGeneratorTests

open NUnit.Framework
open Mandelbrot.MandelbrotCalculator
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
   
    // testPrint(sprintf "Duration : %A" (stopwatch.Elapsed))
   
    stopwatch.Elapsed
  
    
[<Test>]
let ``Performance 2``()=    
    
    performance "test1" (fun () -> graph |> renderSet iterationsToCheck) 
    |> ignore
    performance "test2" (fun () -> graph |> renderSet iterationsToCheck) 
    |> ignore
    performance "test3" (fun () -> graph |> renderSet iterationsToCheck)
    |> ignore

[<Test>]
let ``Tile generation performance - zoom levels 0 to 2``() =
    let fullViewport = { XMin = -2.0; XMax = 2.0; YMin = -2.0; YMax = 2.0 }
    let tilePixelSize = 256
    let tileIterations = 400

    let toViewport x y zoom =
        let cellCount = 2. ** float zoom
        let cellWidth  = (fullViewport.XMax - fullViewport.XMin) / cellCount
        let cellHeight = (fullViewport.YMax - fullViewport.YMin) / cellCount
        { XMin = fullViewport.XMin + cellWidth  * float x
          XMax = fullViewport.XMin + cellWidth  * float (x + 1)
          YMin = fullViewport.YMin + cellHeight * float y
          YMax = fullViewport.YMin + cellHeight * float (y + 1) }

    let renderTile x y zoom =
        let g = Graph(tilePixelSize, tilePixelSize, toViewport x y zoom, tileIterations)
        g |> renderSet tileIterations

    let totalWatch = System.Diagnostics.Stopwatch.StartNew()

    for zoom in 0..3 do
        let cellCount = 1 <<< zoom
        let tileCount = cellCount * cellCount
        let sw = System.Diagnostics.Stopwatch.StartNew()
        for x in 0..cellCount-1 do
            for y in 0..cellCount-1 do
                renderTile x y zoom
        sw.Stop()
        let avgMs = sw.ElapsedMilliseconds / int64 tileCount
        testPrint(sprintf "Zoom %d: %d tile(s) in %dms (avg %dms/tile)" zoom tileCount sw.ElapsedMilliseconds avgMs)

    totalWatch.Stop()
    testPrint(sprintf "Total: %dms" totalWatch.ElapsedMilliseconds)
