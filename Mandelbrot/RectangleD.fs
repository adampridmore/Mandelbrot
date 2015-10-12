namespace Mandelbrot

[<StructuredFormatDisplay("XMin:{XMin}\nXMax:{XMax}\nYMin:{YMin}\nYMax:{YMax}")>]
type RectangleD = 
    {XMin: float; XMax : float ; YMin: float; YMax: float}
    member this.Width = this.XMax - this.XMin
    override this.ToString() = sprintf "%A" this 

    static member translate iterations (fromR:RectangleD) (toR:RectangleD) = 
        let incrementXMin = ( toR.XMin - fromR.XMin)  / (float iterations)
        let incrementXMax = (toR.XMax - fromR.XMax)  / (float iterations)
        let incrementYMin = (toR.YMin - fromR.YMin)  / (float iterations)
        let incrementYMax = (toR.YMax - fromR.YMax)  / (float iterations)

        seq{for i in 0. ..(float iterations-1.) do
            yield {
                RectangleD.XMin = fromR.XMin + (incrementXMin * i)
                XMax = fromR.XMax + (incrementXMax * i)
                YMin = fromR.YMin + (incrementYMin * i)
                YMax = fromR.YMax + (incrementYMax * i)
            }
        }
