module GraphHelpers

let mapValueToPixel (min:float) (max:float) (pixelWidth:float) (valueToMap:float) =
    ((valueToMap - min) / (max - min)) * pixelWidth

let mapPixelToValue (min:float) (max:float) (width:int) (pixelValue:int) = 
    let percent = (float pixelValue) / (float width)
    ((max - min) * percent) + min
