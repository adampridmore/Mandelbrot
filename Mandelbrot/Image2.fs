module Mandelbrot.Image2

open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open Mandelbrot.Color

let imageTypeExtension = "png"
let encoder = new SixLabors.ImageSharp.Formats.Png.PngEncoder()

type Bitmap3(width: int32, height: int32) =
  let image = new Image<Rgba32>(width, height)

  member this.Bitmap = image

  member _.setPixel(x :int, y:int, c: Color2) : Unit =
    image[x,y] <- Rgba32(c.R(),c.G(),c.B())

  member this.Save(stream: Stream) : Unit =
    image.Save(stream, encoder)
