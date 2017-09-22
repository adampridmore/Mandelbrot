module TileTests

open FsUnit
open NUnit.Framework

open Tile
open Types
open Mercator
   
[<Test>]
let ``map to pixel for 0 0 0 tile at 0 , 0``() = 
    let tile = new Tile(0,0,0,100)
    
    let c = {longitude=0.0;latitude=0.0}

    tile.toTilePixelOption c 
    |> should equal (Some {x=50;y=50} )

[<Test>]
let ``map to pixel for 0 0 0 tile at -90 , 0``() = 
    let tile = new Tile(0,0,0, 100)
    
    let c = {longitude= -90.0;latitude=0.0}

    tile.toTilePixelOption c 
    |> should equal (Some {x=25;y=50} )

[<Test>]
let ``map to pixel for 0 0 0 tile at 0 , 45``() = 
    let tile = new Tile(0,0,0, 100)
    
    let c = {longitude=0.0;latitude=45.0}

    tile.toTilePixelOption c 
    |> should equal (Some {x=50;y=35})

[<Test>]
let ``map to pixel for 0 0 1 tile at 0 , 0``() = 
    let tile = new Tile(0,0,1,100)
    
    let c = {longitude= -90.0 ;latitude=45.0}

    tile.toTilePixelOption c 
    |> should equal (Some {x=50;y=71} )

[<Test>]
let ``map to pixel for 0 0 1 tile when not in tile``() = 
    let tile = new Tile(0,0,1,100)
    
    let c = {longitude= 90.0 ;latitude=0.0}

    tile.toTilePixelOption c 
    |> should equal None


[<Test>]
let ``map to pixel for real zoomed in tile with real lat long``() = 
    let tile = new Tile(8096,5264,14, 256)
    
    let c = {longitude= -2.0883150 ;latitude=53.9524170}

    tile.toTilePixelOption c 
    |> should equal (Some {x=245;y=72} )