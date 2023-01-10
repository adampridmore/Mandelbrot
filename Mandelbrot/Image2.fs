module Mandelbrot.Image2

// open System.Drawing

open System.IO

// type Bitmap2(width: int32, height: int32) = 
//   let bitmap: Bitmap = new Bitmap(width, height) 

//   member this.Bitmap = bitmap

//   member this.setPixel(x :int, y:int, c: Color) : Unit =
//     lock this.Bitmap (fun () -> this.Bitmap.SetPixel(x,y,c) )

//   member this.Save(stream: Stream, format: Imaging.ImageFormat) : Unit = 
//     this.Bitmap.Save(stream, format)

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open Mandelbrot.Color

// TODO: Change to png
let imageTypeExtension = "png"

type Bitmap3(width: int32, height: int32) = 
  let encoder = new Formats.Png.PngEncoder()
  
  let image = new Image<Rgba32>(width, height)

  member this.Bitmap = image

  member this.setPixel(x :int, y:int, c: Color2) : Unit =
    lock this.Bitmap (fun () -> image[x,y] <- Rgba32(c.R(),c.G(),c.B()))

  member this.Save(stream: Stream) : Unit = 
    image.Save(stream, encoder)

  member this.Save(fileName: string) : Unit = 
    image.Save(fileName, encoder)
