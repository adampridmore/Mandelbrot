module PointD

type PointD = 
    {X: float; Y: float } 
    member this.ToPointF = new System.Drawing.PointF(float32 this.X, float32 this.Y)
    member this.ToPoint = new System.Drawing.Point(int this.X, int this.Y)

let Zero = {X=0.;Y=0.}
