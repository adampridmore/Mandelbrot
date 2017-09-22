module BitmapHelpers

open System.Drawing

let applyColorIncrement alphaIncrement (color:Color)  = 
    let newAlpha = 
        match (color.A |> int) + alphaIncrement with
        | a when a > 255 -> 255
        | a -> a

    Color.FromArgb(newAlpha, color)

let toBitmapCoordinates(bitmap:Bitmap) = 
    seq{
        for x in 0 .. bitmap.Size.Width-1 do
            for y in 0 .. bitmap.Size.Height-1 do
                yield (x,y)
    }

let fillImage backgroundColor (bitmap:Bitmap) = 
    bitmap 
    |> toBitmapCoordinates 
    |> Seq.iter (fun (x,y) -> bitmap.SetPixel(x, y, backgroundColor))

    bitmap

let getPixelColor pixel (bitmap:Bitmap) = bitmap.GetPixel pixel
let setPixelColor (x,y) (bitmap:Bitmap) (color: Color) = bitmap.SetPixel(x,y, color)

let applyToPixel (pixel) fn (bitmap:Bitmap) =
    bitmap 
    |> getPixelColor pixel 
    |> fn 
    |> setPixelColor pixel bitmap

