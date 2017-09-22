module Mercator
open System;

let PI = Math.PI
let Exp = Math.Exp
let Log = Math.Log
let Tan = Math.Tan
let Atan = Math.Atan
let Pow x y = Math.Pow(x,y)

// From:
// http://gis.stackexchange.com/a/66357
let FromLatToY lat tileSize = 
    (1.0 - Log(Tan(lat * PI / 180.0) + 1.0 / cos(lat * PI / 180.0)) / PI) / 2.0 * (Pow 2.0 0.0) * tileSize

let tileWidthCount zoom = (2. ** (zoom |> float)) |> int
