module Mandelbrot.TileGenerator

open System.Drawing
open System.Drawing.Imaging

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
    
    let bitmap = new Bitmap(tileSize, tileSize, PixelFormat.Format32bppArgb)
    
    use g = Graphics.FromImage(bitmap)

    renderBox g
    renderText g

    bitmap
