module MercatorTest

open FsUnit
open NUnit.Framework
open Mercator

[<Test>]
let ``Tile width count for zoom 0 is 1``() = 
    tileWidthCount 0 |> should equal 1

[<Test>]
let ``Tile width count for zoom 1 is 2``() = 
    tileWidthCount 1 |> should equal 2

[<Test>]
let ``Tile width count for zoom 2 is 4``() = 
    tileWidthCount 2 |> should equal 4



