module GraphHelpers

let mapToPixelValue (minX:float) (maxX:float) (pixelWidth:float) (valueToMap:float) =
    ((valueToMap - minX) / (maxX - minX)) * pixelWidth

let pixelIncrement (minX:float) (maxX:float) (pixelWidth:float) = 
    (maxX - minX) / pixelWidth

let pixelToValue (min:float) (max:float) (width:int) (pixelValue:int) = 
    let percent = (float pixelValue) / (float width)
    ((max - min) * percent) + min
