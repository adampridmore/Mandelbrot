#r @"..\packages\FSharp.Charting.0.90.12\lib\net40\FSharp.Charting.dll"
#r "System.Windows.Forms.DataVisualization.dll"


open System.Numerics
open System.Drawing
open FSharp.Charting

//let a = new Complex(2. , 3.)
//let b = new Complex(3. , 0.)
//let c = a * b
//
//printfn "%A" c

let nextNum initialValue currentValue =
    initialValue + currentValue * currentValue

let fseq initialValue = 
    let folder state = 
        let x = nextNum initialValue state 
        Some(x,x)
    Seq.unfold folder initialValue
    
let numseq start inc =
    let action state = 
        let newState = state + inc
        Some(newState, newState)
    Seq.unfold action start

//numseq 10. 0.01
//|> Seq.take 20
//|> Seq.iter (printfn "%A")

//let x = -1.5 //new Complex(1.,0.5) //-1.5
//fseq x
////|> Seq.skip 100
//|> Seq.take 10
//|> Seq.iter (printfn "%A")

let showChart (chart:ChartTypes.GenericChart) = chart.ShowChart()

Chart.Line([ for x in 0 .. 10 -> x, x*x ])
|> showChart

let curvyData = [ for i in 0.0 .. 0.02 .. 2.0 * System.Math.PI -> (sin i, cos i * sin i) ] 
(curvyData |> Chart.Line).ShowChart()

let rnd = new System.Random()
let rand() = rnd.NextDouble()
let randomPoints = [ for i in 0 .. 1000 -> rand(), rand() ]
(Chart.Point randomPoints).ShowChart()

[ for i in 0 .. 1000 -> rand(), rand() ] 
|> Chart.Point 
|> showChart

let c = (   [ for i in 0 .. 1000 -> rand(), rand() ] 
            |> Chart.Point )

//[ for i in 0 .. 1000 -> rand(), rand() ] |> Chart.Point |> showChart
//[ for i in 0 .. 1000 -> rand(), rand() ] |> Chart.Point |> showChart



//numseq -1. 0.1
//|> Seq.take 10
//|> Seq.map (fun x -> fseq x |> Seq.take 10)
//|> Seq.iter (printfn "%A")

