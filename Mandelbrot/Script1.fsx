#r "System.Drawing.dll"
#r "..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"

#load "ColorModule.fs"
#load "PointD.fs"
#load "GraphHelpers.fs"
#load "Mandelbrot.fs"
#load "Graph.fs"

open System.Numerics
open System.Drawing
open MandelbrotCalc

let renderSet filename iterationsToCheck = 
    let graph = new Graph.Graph(3840, 2160,-1.15590493422159, -1.15554362720591, 0.277349331271918, 0.278131835829461)
    //let graph = new Graph.Graph(1024, 768,-2.,2.,-2.,2.)
    //graph.DrawAxes()
    graph.RenderSet()
    graph.Bitmap.Save(filename)

let iterationsToCheck = 400
let filename = sprintf @"C:\temp\mandlebrot\%d.bmp" iterationsToCheck
renderSet filename iterationsToCheck

System.Diagnostics.Process.Start(filename)