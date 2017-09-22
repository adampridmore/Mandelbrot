#I @"..\PaddingtonRepository\bin\Debug"
#I @"..\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40"

#r @"PaddingtonRepository.dll"
#r @"FSharp.PowerPack.Parallel.Seq.dll"
#r @"System.Drawing.dll"

#load "Types.fs"
#load "Mercator.fs"
#load "Tile.fs"
#load "BitmapHelpers.fs"

open Types
open PaddingtonRepository;
open System.Drawing
open BitmapHelpers
open Microsoft.FSharp.Collections

let tileSetName = Domain.Tile.Heatmaps5HarshBraking

//let mrPointsCollectionName = "routeResult_mr_points"
let mrPointsCollectionName = "driverBehaviourEvent_mr_points"
                             
// Local
let heatmap_data_mongoUri = "mongodb://localhost/heatmap_data"
let tilesMongoUri = "mongodb://localhost/tiles"

// AWS MongoDB
//let heatmap_data_mongoUri = "mongodb://10.180.2.211/heatmap_data"
//let tilesMongoUri = "mongodb://10.180.2.211/tiles"

let pointsRepository = new PaddingtonRepository.MrPointsRepository(heatmap_data_mongoUri, mrPointsCollectionName)
let tileRepository = new PaddingtonRepository.TileRepository(tilesMongoUri)

let tileSize = 256
let pixelColor = Color.FromArgb(0, System.Drawing.Color.Red)
let imageFormat = System.Drawing.Imaging.ImageFormat.Png

let toTile (tile:PaddingtonRepository.Domain.Tile) = Tile.Tile(tile.X, tile.Y, tile.Zoom, tileSize)

let toCoordinate (p:Domain.Point) = { latitude = p.Latitude; longitude = p.Longitude}

let imageToData (image: Bitmap) =
    let ms = new System.IO.MemoryStream()
    image.Save(ms, imageFormat)
    ms.ToArray()

let zoomToAlphaIncrement zoom = 
    match zoom with
    | z when z <= 5 -> 1
    | 6 -> 2
    | 7 -> 4
    | 8 -> 8
    | 9 -> 16
    | 10 -> 32
    | 11 -> 64
    | 12 -> 128
    | _ -> 255

//let toEventStatusColor (tile:PaddingtonRepository.Domain.TileWithPoints) = 
//    match tile.EventStatus with
//    | "behaviourExcessiveIdling" -> Color.Yellow
//    | "behaviourHarshCornering" -> Color.Blue
//    | "behaviourEngineOverRevving" -> Color.Orange
//    | "behaviourOverSpeed" -> Color.Red
//    | "behaviourHarshAcceleration" -> Color
//    | "behaviourHarshBraking" -> Color.Red
//    | _ -> Color.FromArgb(0,)

let renderTile (tile:PaddingtonRepository.Domain.TileWithPoints) =
    
    let bitmap = new Bitmap(tileSize, tileSize)|> fillImage pixelColor 
       
    let renderTile = tile.Tile |> toTile
    
    printfn "Z:%d,X:%d,Y:%d, Points: %d" tile.Tile.Zoom tile.Tile.X tile.Tile.Y tile.Points.Count

    //let colorMerge(_) = tile |> toEventStatusColor
    let alphaIncrementFn = tile.Tile.Zoom |> zoomToAlphaIncrement |> applyColorIncrement
    
    tile.Points 
    |> Seq.map toCoordinate
    |> Seq.map renderTile.toTilePixelOption
    |> Seq.choose id
    |> Seq.iter (fun pixel -> applyToPixel (pixel.x, pixel.y) alphaIncrementFn bitmap)

    // Yuk - mutable. Pleeeease....
    tile.Tile.Data <- (bitmap |> imageToData)
    tile.Tile.TileSetName <- tileSetName
    tile.Tile.CreatedDateTime <- (System.DateTime.UtcNow)
    tile.Tile

let saveTileToRepo tile = tileRepository.Save(tile)

let createFilename zoom x y = 
    let fileName = System.IO.Path.GetTempFileName()
    sprintf "%s_Z%d_X%d_Y%d.%s" fileName zoom x y (imageFormat.ToString())

let saveTileToDisk (fileName:string) (tile:Domain.Tile) = 
    System.IO.File.WriteAllBytes(fileName, tile.Data)
    printfn "Filename: %s" fileName
    fileName

let showTile (fileName:string) = 
    fileName |> System.Diagnostics.Process.Start |> ignore

let additionalMrPointsFiltering = 
    null
    //"behaviourHarshBraking"

let getTileForZoom zoom x y = 
    match pointsRepository.TryGetTileForZoom(zoom,x,y, additionalMrPointsFiltering) with
    | null -> None
    | t -> Some(t)

// Render all tiles sets
// Use PSeq?
let renderZoomLevel zoom =
    pointsRepository.GetTilesForZoom(zoom, additionalMrPointsFiltering)
    |> PSeq.map renderTile
    |> Seq.iter saveTileToRepo

//#time "on"

let renderAllTilesForTileSet() = 
    let stopWatch = System.Diagnostics.Stopwatch.StartNew();
    
//    printfn "Deleting tileset: %s" tileSetName
//    tileRepository.DeleteTileSet tileSetName

    {0..15}
    //{3..4}
    |> Seq.iter renderZoomLevel
    printfn "Duration %ds" (stopWatch.Elapsed.TotalSeconds |> int)

let renderThreeSampleTiles() =
    // Render three sample tiles
    let renderSpecificTile zoom x y = 
        match getTileForZoom zoom x y with
        | None -> (printfn "no tile found in DB: z:%d x:%d y:%d" zoom x y); ()
        | Some(t) -> t  |> renderTile
                        |> saveTileToDisk (createFilename zoom x y)
                        //|> ignore
                        |> showTile

    [   
    //    (0,  0,   0);       // All
        //(5,  15,   10);     // West UK
        (11, 1015, 659);    // Leeds
        (13, 4065, 2637)    // Aberford
    ]
    |> Seq.iter (fun (x,y,zoom) -> renderSpecificTile x y zoom)

    //// X, Y, Zoom
    //// 15, 10, 5       
    //// 1015, 659, 11    
    //// 4654, 2637, 13  

//renderAllTilesForTileSet()
renderThreeSampleTiles()
