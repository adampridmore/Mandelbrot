module PaddingtonHeatmapsComponents.TileGenerator

open System.Drawing
open System.Drawing.Imaging
open Types

let tileSize = 256
let alpha = 80

let generateTile x y zoom = 
    let boxLines = 
        seq{
            let tileSizeF = tileSize |> float32

            yield new PointF(0.0f, 0.0f)
            yield new PointF(0.0f, tileSizeF - 1.0f)
            yield new PointF(tileSizeF - 1.0f, tileSizeF - 1.0f)
            yield new PointF(tileSizeF - 1.0f, 0.0f)
            yield new PointF(0.0f, 0.0f)
        }

    let alphaBrush = new SolidBrush(Color.FromArgb(alpha, Color.DeepSkyBlue))
    let alphaBrushRed= new SolidBrush(Color.FromArgb(alpha, Color.Red))
        
    let renderWatermark (g:Graphics) = 
        g.DrawLine(new Pen(alphaBrush, 5.f), 0, 0, tileSize - 1, tileSize - 1)
        g.FillEllipse(alphaBrush, 64, 64, tileSize - 129, tileSize - 129)

    let renderText (g:Graphics) = 
        let textToDraw = sprintf "%d, %d, %d" x y zoom;
        let font = new Font(SystemFonts.DefaultFont.FontFamily, 10.0f)
        g.DrawString(textToDraw, font, alphaBrush, 10.0f, 10.0f)

    let renderBox (g:Graphics) = 
        g.DrawLines(new Pen(alphaBrush), (boxLines |> Seq.toArray) )
    
    let renderCoordinate (g:Graphics) (b:Bitmap) c =
        let t = Tile.Tile(x,y,zoom,tileSize)
//        let pixel = Tile.mapToPixel tileSize t c 
        let pixel = t.toTilePixelOption c
        match pixel with
        | Some(pixel) -> b.SetPixel(pixel.x,pixel.y, Color.Red)
        | None -> ()

    let pixelToPoint (pixel:Pixel) : (PointF) = new PointF(pixel.x |> float32, pixel.y |> float32)

    let renderLine (g:Graphics) c1 c2 =
        let t = Tile.Tile(x,y,zoom,tileSize)
        let pixel1 = t.toTilePixel c1
        let pixel2 = t.toTilePixel c2
        let p1 = pixel1 |> pixelToPoint
        let p2 = pixel2 |> pixelToPoint
        g.DrawLine(new Pen(alphaBrushRed,3.0f), p1, p2)
        

    let bitmap = new Bitmap(tileSize, tileSize, PixelFormat.Format32bppArgb)
    
    use g = Graphics.FromImage(bitmap)

    renderBox g
    //renderWatermark g
    renderText g

//    Data.points 
//    |> Seq.map (fun (long, lat) -> {longitude = long; latitude = lat})
//    |> Seq.iter (renderCoordinate g bitmap)

//    Data.points 
//    |> Seq.map (fun (long, lat) -> {longitude = long; latitude = lat})
//    |> Seq.pairwise
//    |> Seq.iter (fun (c1, c2) -> renderLine g c1 c2)
    
    
//    renderCoordinate g bitmap { 
//        longitude = -1.8666755;
//        latitude = 53.8277477;
//    }

    bitmap
