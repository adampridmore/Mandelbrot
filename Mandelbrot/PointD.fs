namespace Mandelbrot

open System.Numerics
open Mandelbrot

type PointD = 
    {X: float; Y: float } 
    member this.ToPixel = { Pixel.X = (int this.X) ; Pixel.Y = (int this.Y) }
    member this.ToComplex = Complex(this.X, this.Y)

    static member Zero = {X=0.;Y=0.}
