namespace Mandelbrot 

open System.Drawing

open System.IO

type Bitmap2(width: int32, height: int32) = 
  let bitmap: Bitmap = new Bitmap(width, height) 

  member this.Bitmap = bitmap

  member this.setPixel(x :int, y:int, c: Color) : Unit =
    lock this.Bitmap (fun () -> this.Bitmap.SetPixel(x,y,c) )

  member this.Save(stream: Stream, format: Imaging.ImageFormat) : Unit = 
    this.Bitmap.Save(stream, format)

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

type Bitmap3(width: int32, height: int32) = 
  let image = new Image<Rgba32>(width, height)

  let encoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder()
    
  member this.Bitmap = image

  member this.setPixel(x :int, y:int, c: System.Drawing.Color) : Unit =
    
    let r = c.R
    let g = c.G
    let b = c.B

    lock this.Bitmap (fun () -> image[x,y] <- Rgba32(r,g,b))

  member this.Save(stream: Stream, format: Imaging.ImageFormat) : Unit = 
    image.Save(stream,encoder)
