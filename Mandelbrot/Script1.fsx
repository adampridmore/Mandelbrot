open System.Numerics
open System.Drawing


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
            
let g = new Graph(400, 300, -1., 1., 0., 10.)

let filename = "c:\\temp\\mandebrot.jpg"

g.DrawLine 0
g.Bitmap.Save(filename, Imaging.ImageFormat.Jpeg)

System.Diagnostics.Process.Start(filename)

//numseq -1. 0.1
//|> Seq.take 10
//|> Seq.map (fun x -> fseq x |> Seq.take 10)
//|> Seq.iter (printfn "%A")

