module GraphTests
open NUnit.Framework
open FsUnit
open RectangleD

[<Test>]
let test() = 
    let view = {RectangeD.XMin = 3. ; XMax = 2.; YMin = 0. ; YMax = 1.}
    let g = new Graph.Graph(3,2,view)

    g.IterateGraph (fun p -> (printfn "%A" p) ; Some(1))
