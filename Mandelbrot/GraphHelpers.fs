module GraphHelpers

open NUnit.Framework
open FsUnit

let mapToPixelValue minX maxX pixelWidth valueToMap =
    ((valueToMap - minX) / (maxX - minX)) * pixelWidth

[<Test>]
let simple_mapping() = 
    let minX = 100.
    let maxX = 200.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 500.

[<Test>]
let simple_mapping_2() = 
    let minX = 100.
    let maxX = 500.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 125.

[<Test>]
let simple_mapping_3() = 
    let minX = -100.
    let maxX = 100.
    let pixelWidth = 1000.

    let valueToMap = 0.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 500.



    