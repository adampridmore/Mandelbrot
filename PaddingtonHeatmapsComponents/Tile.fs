module Tile
open Types
open Mercator

let Pow = System.Math.Pow

type Tile (x:int, y:int, zoom:int, tilePixelSize:int) = 
    let filterInvalidTile = 
        function
        | p when p.x < 0 || p.x >= tilePixelSize -> None
        | p when p.y < 0 || p.y >= tilePixelSize -> None
        | pixel -> Some(pixel)
        
    member this.totalNumberOfTiles = zoom |> tileWidthCount 
    member this.totalTileLayerPixelWidth = this.totalNumberOfTiles * tilePixelSize
    member this.toTileLayerPixel (coordinate:Coordinate) = 
        let x = (this.totalTileLayerPixelWidth |> float)* (coordinate.longitude + 180.0) / 360.0 |> int

        let y = 
            FromLatToY coordinate.latitude (this.totalTileLayerPixelWidth |> float)
            |> int
        { x=x;y=y}

    member this.toTilePixel (c:Coordinate) = 
        let layerPixel = c |> this.toTileLayerPixel
        
        {   x=layerPixel.x - (x * tilePixelSize);
            y=layerPixel.y - (y * tilePixelSize) }
        
    member this.toTilePixelOption (c:Coordinate) = 
        this.toTilePixel c |> filterInvalidTile 

        
                  


