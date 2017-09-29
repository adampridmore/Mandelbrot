module Tile
open Types

let Pow = System.Math.Pow

let tileWidthCount zoom = (2. ** (zoom |> float)) |> int

type Tile (x:int, y:int, zoom:int, tilePixelSize:int) = 
    let filterInvalidTile = 
        function
        | p when p.x < 0 || p.x >= tilePixelSize -> None
        | p when p.y < 0 || p.y >= tilePixelSize -> None
        | pixel -> Some(pixel)
        
    member this.totalNumberOfTiles = zoom |> tileWidthCount 
    member this.totalTileLayerPixelWidth = this.totalNumberOfTiles * tilePixelSize
        
                  


