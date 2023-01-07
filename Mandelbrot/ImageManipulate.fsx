#r "nuget: SixLabors.ImageSharp, 2.1.3"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.PixelFormats

let fn() : Unit = 

  use image = new Image<Rgba32>(10,10)

  image[5,5] <- Rgba32(100uy,255uy,255uy)

  image.Save("image.bmp")

  printfn "Pixel 1: %O" image[0,0]
  printfn "Pixel 2: %O" image[5,5]

  // image.Mutate(x => x.Fill(Rgba32.HotPink))

fn()
