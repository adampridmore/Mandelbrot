module GraphHelpers

let mapToPixelValue (min:float) (max:float) (pixelWidth:float) (valueToMap:float) =
    ((valueToMap - min) / (max - min)) * pixelWidth

let pixelToValue (min:float) (max:float) (width:int) (pixelValue:int) = 
    let percent = (float pixelValue) / (float width)
    ((max - min) * percent) + min
