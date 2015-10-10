module GraphTests
open NUnit.Framework
open FsUnit

[<Test>]
let test() = 
    let g = new Graph.Graph(3,2,0.,2.,0.,1.)

    g.IterateGraph (fun p -> (printfn "%A" p) ; Some(1))
