module PointD
open System.Drawing

type PointD = 
    {X: float; Y: float } 
    member this.ToPointF = new PointF(float32 this.X, float32 this.Y)
    member this.ToPoint = new Point(int this.X, int this.Y)
    static member FromPoint (point:Point) = {X=(float point.X); Y=(float point.Y)}

let Zero = {X=0.;Y=0.}
