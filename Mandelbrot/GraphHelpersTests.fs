module GraphHelpersTests

open NUnit.Framework
open FsUnit
open GraphHelpers


[<Test>]
let ``simple mapping``() = 
    let minX = 100.
    let maxX = 200.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 500.

[<Test>]
let ``simple mapping 2``() = 
    let minX = 100.
    let maxX = 500.
    let pixelWidth = 1000.

    let valueToMap = 150.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 125.

[<Test>]
let ``simple mapping 3``() = 
    let minX = -100.
    let maxX = 100.
    let pixelWidth = 1000.

    let valueToMap = 0.
    
    mapToPixelValue minX maxX pixelWidth valueToMap |> should equal 500.

[<Test>]
let ``pixel increment size``()=
    let minX = -100.
    let maxX = 100.
    let pixelWidth = 100.
        
    let value = pixelIncrement minX maxX pixelWidth
    
    value |> should equal 2. 

[<Test>]
let ``map pixel to value``()=
    pixelToValue -100. 200. 900 600 |> should equal 100 

