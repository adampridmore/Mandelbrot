#load "Mercator.fs"

open Mercator

let toTileIndexX zoom long =
    let percent = (long + 180.) / 360.
    let totalTileCount = zoom |> tileWidthCount 
    (int (float totalTileCount * percent)) % totalTileCount

let toTileIndexY zoom lat =
    let totalTileCount = zoom |> tileWidthCount |> float
    FromLatToY lat totalTileCount |> int

let zoom = 10

//[   -181.;-180.;-179.;
//    -91.;-90.;-89.;
//    -2.-1.;0.;1.;
//    89.;90.;91.;
//    179.;180.;
//]
Seq.singleton -2.
|> Seq.map (fun x -> (x,x |> toTileIndexX zoom))
|> Seq.iter (printfn "%A")

//zoom |> tileWidthCount 
//FromLatToY -50. (zoom |> tileWidthCount |> float) |> int

toTileIndexX zoom -2.
toTileIndexY zoom 54.
