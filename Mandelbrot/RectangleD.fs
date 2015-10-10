module RectangleD

[<StructuredFormatDisplay("XMin:{XMin}\nXMax:{XMax}\nYMin:{YMin}\nYMax:{YMax}")>]
type RectangeD = 
    {XMin: float; XMax : float ; YMin: float; YMax: float}
    member this.Width = this.XMax - this.XMin
    override this.ToString() = sprintf "%A" this 

let translate iterations (fromR:RectangeD) (toR:RectangeD) = 
    let incrementXMin = ( toR.XMin - fromR.XMin)  / (float iterations)
    let incrementXMax = (toR.XMax - fromR.XMax)  / (float iterations)
    let incrementYMin = (toR.YMin - fromR.YMin)  / (float iterations)
    let incrementYMax = (toR.YMax - fromR.YMax)  / (float iterations)

    seq{for i in 0. ..(float iterations-1.) do
        yield {
            RectangeD.XMin = fromR.XMin + (incrementXMin * i)
            RectangeD.XMax = fromR.XMax + (incrementXMax * i)
            RectangeD.YMin = fromR.YMin + (incrementYMin * i)
            RectangeD.YMax = fromR.YMax + (incrementYMax * i)
        }
    }