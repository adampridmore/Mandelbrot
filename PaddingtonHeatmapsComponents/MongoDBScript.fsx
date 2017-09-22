#r @"..\Packages\MongoDB.Bson.2.2.3\lib\net45\MongoDB.Bson.dll"
#r @"..\Packages\MongoDB.Driver.2.2.3\lib\net45\MongoDB.Driver.dll"
#r @"..\Packages\MongoDB.Driver.Core.2.2.3\lib\net45\MongoDB.Driver.Core.dll"
#r @"..\PaddingtonRepository\bin\Debug\PaddingtonRepository.dll"

open PaddingtonRepository.Domain

let mongoUri = "mongodb://localhost/test"

let saveTileToCollection tile = 
    let r = PaddingtonRepository.TileRepository(mongoUri)
    tile |> r.Save
    tile
    
type ParsedTile = 
    | InvaidFilename of string
    | TileParams of int * int * int * string

let filenameToTileParams filename =
    let regex = System.Text.RegularExpressions.Regex("([0-9])+")
    let matches = regex.Matches(filename |> System.IO.Path.GetFileNameWithoutExtension)
    match matches with 
    | m when m.Count < 3 -> InvaidFilename(filename)
    | m -> 
        let zoom =  matches.[0].Value |> int
        let x = matches.[1].Value |> int
        let y = matches.[2].Value |> int
        TileParams(zoom,x,y, filename)

let toTile (zoom,x,y,filename) =
    let data = System.IO.File.ReadAllBytes filename
    let tile = Tile()
    tile.Id <- filename
    tile.X <- x
    tile.Y <- y
    tile.Zoom <- zoom
    tile.Data <- data
    tile
    
let deleteTile (tile:Tile) = 
    System.IO.File.Delete(tile.Id)
    tile

let printTile (tile:Tile) = 
    printfn "%s" tile.Id
    tile

@"C:\temp\mandelbrot"
//@"Z:\9"
//@"C:\Dev\PaddingtonHeatMaps\PaddingtonHeatMaps\Content\themes\base\images\Tiles3"
//@"C:\Users\Adam\Dropbox\Work\Dev\PaddingtonHeatMaps\PaddingtonHeatMaps\Content\themes\base\images\Tiles3"
|> System.IO.Directory.GetFiles
|> Seq.sort
|> Seq.map filenameToTileParams
|> Seq.map (function InvaidFilename(_) -> None | TileParams(a,b,c,d) -> Some(a,b,c,d))
|> Seq.choose id
|> Seq.map toTile 
//|> Seq.chunkBySize 100
//|> Seq.iter saveTilesToCollection
|> Seq.map saveTileToCollection
|> Seq.map deleteTile
|> Seq.iter (printTile >> ignore)
