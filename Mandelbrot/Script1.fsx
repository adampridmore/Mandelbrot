open System.Numerics

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

let x = -1.5 //new Complex(1.,0.5) //-1.5
fseq x
//|> Seq.skip 100
|> Seq.take 10
|> Seq.iter (printfn "%A")
