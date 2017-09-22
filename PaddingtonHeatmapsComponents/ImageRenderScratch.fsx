#r @"System.Drawing.dll"

#load @"BitmapHelpers.fs"

open System.Drawing
open BitmapHelpers

let imageSize = 256
let imateFormat = System.Drawing.Imaging.ImageFormat.Png

let createFilename = 
    let fileName = System.IO.Path.GetTempFileName()
    sprintf "%s.%s" fileName (imateFormat.ToString())

let shellExecute (fileName:string) =  fileName |> System.Diagnostics.Process.Start

let fileName = createFilename

let bitmap = new Bitmap(imageSize, imageSize)

let backgroundColor = Color.FromArgb(0, 0,0,255)
let alphaIncrement = 10

let processPixel (bitmap:Bitmap) pixel = 
    bitmap 
    |> applyToPixel pixel (applyColorIncrement alphaIncrement)


bitmap |> fillImage backgroundColor

bitmap
|> toBitmapCoordinates
|> Seq.filter (fun (x,y) -> x < (imageSize / 2))
|> Seq.iter (processPixel bitmap)

bitmap 
|> toBitmapCoordinates
|> Seq.filter (fun (x,y) -> x < (imageSize / 4))
|> Seq.iter (processPixel bitmap)

bitmap 
|> toBitmapCoordinates
|> Seq.filter (fun (x,y) -> x < (imageSize / 8))
|> Seq.iter (processPixel bitmap)

bitmap 
|> toBitmapCoordinates
|> Seq.filter (fun (x,y) -> x < (imageSize / 16))
|> Seq.iter (processPixel bitmap)

bitmap.Save(fileName, imateFormat)

fileName |> shellExecute




