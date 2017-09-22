#load "Mercator.fs"
#load "Types.fs"
#load "Tile.fs"

open Mercator
open Types

let tilePixelSize = 100
let latitude = 45.0
let tile = Tile.Tile(7,5,4,tilePixelSize)

let c = {longitude = -2.0; latitude = 53.0}
tile.totalNumberOfTiles
tile.totalTileLayerPixelWidth
tile.toTileLayerPixel c
tile.toTilePixel c
