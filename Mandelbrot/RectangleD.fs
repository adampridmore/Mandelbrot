namespace Mandelbrot

[<StructuredFormatDisplay("XMin:{XMin}\nXMax:{XMax}\nYMin:{YMin}\nYMax:{YMax}")>]
type RectangleD = 
    {XMin: float; XMax : float ; YMin: float; YMax: float}
    
    member this.Width = this.XMax - this.XMin
    member this.Height = this.YMax - this.YMin
    member this.Area = this.Width * this.Height
    member this.AspectRatio = this.Height / this.Width
    member this.CenterX = this.XMin + (this.Width / 2.)
    member this.CenterY = this.YMin + (this.Height / 2.)
    
    override this.ToString() = sprintf "%A" this 

    static member TranslationSeq iterations (fromR:RectangleD) (toR:RectangleD) =
            RectangleD.TranslationSeq3 iterations fromR toR 

    static member TranslationSeq1 iterations (fromR:RectangleD) (toR:RectangleD) =
        let incrementXMin = (toR.XMin - fromR.XMin)  / (float iterations)
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

    static member CalculateZoomSequence iterations (fromR:RectangleD) (toR:RectangleD) =
        let calculateDeltaPercent v1 v2 iterations = 
            System.Math.Pow((v2 / v1), 1. / (float iterations))

        let areaDeltaIncrease = calculateDeltaPercent fromR.Area toR.Area iterations
        let aspectRatioDeltaIncrease = calculateDeltaPercent fromR.AspectRatio toR.AspectRatio iterations

        let sqrt x = System.Math.Sqrt(x)
        let newWidth a r = a / sqrt(a * r)
        let newHeight a r = sqrt(a*r)

        Seq.unfold (fun (a,r) -> let newValue = (a*areaDeltaIncrease, r*aspectRatioDeltaIncrease)
                                 Some(newValue, newValue))
                                 (fromR.Area, fromR.AspectRatio)
        |> Seq.take iterations
        |> Seq.map (fun (a, ar) -> ( (newWidth a ar), (newHeight a ar) ) )

    static member TranslationSeq3 iterationsInt (fromR:RectangleD) (toR:RectangleD) =
        let sqrt x = System.Math.Sqrt(x)
        
        let calculateDeltaPercent v1 v2 iterations =
            match (v1,v2) with
            | (v1,v2) when v1 = v2 -> 1. 
            | (v1, v2) -> System.Math.Pow((v2 / v1), 1. / (float iterations))

        let iterations = float iterationsInt

        let areaDelta = calculateDeltaPercent fromR.Area toR.Area iterations
        let aspectRatioDelta = calculateDeltaPercent fromR.AspectRatio toR.AspectRatio iterations
        let centerXDelta = (toR.CenterX - fromR.CenterX) / iterations
        let centerYDelta = (toR.CenterY - fromR.CenterY) / iterations
        let newWidth a r = a / sqrt(a * r)
        let newHeight a r = sqrt(a*r)

        let action (iteration, previousR:RectangleD) = 
            let nextArea = previousR.Area * areaDelta
            let nextAspectRatio = previousR.AspectRatio * aspectRatioDelta
            let nextWidth = newWidth nextArea nextAspectRatio
            let nextHeight = newHeight nextArea nextAspectRatio
            let nextCenterX = previousR.CenterX + centerXDelta
            let nextCenterY = previousR.CenterY + centerYDelta
            let nextR = { 
                RectangleD.XMin = nextCenterX - (nextWidth / 2.)
                RectangleD.YMin = nextCenterY - (nextHeight / 2.)
                RectangleD.XMax = nextCenterX + (nextWidth / 2.)
                RectangleD.YMax = nextCenterY + (nextHeight / 2.)
            }
            Some(nextR, (iteration + 1, nextR))

        Seq.unfold action (0, fromR)
        |> Seq.take iterationsInt
