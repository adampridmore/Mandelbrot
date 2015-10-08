module GraphTests
open NUnit.Framework
open FsUnit

[<Test>]
let test() = 
    let g = new Graph.Graph(3,2,0.,2.,0.,1.)

    g.IterateGraph (fun x y -> (printfn "%fx%f" x y) ; Some(1))
