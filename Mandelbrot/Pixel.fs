namespace Mandelbrot

open System.Drawing

type Pixel = 
    {X: int; Y: int}
    static member FromPoint (point:Point) = {X=(point.X); Y=(point.Y)}
