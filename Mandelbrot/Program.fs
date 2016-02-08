module Program

//open Mandelbrot
//open Mandelbrot.MandelbrotCalculator
//    
//let parseOrDefault text defaultValue = 
//    let mutable value = 0;
//    if System.Int32.TryParse(text, &value) then value
//    else defaultValue
//
//let parseOrDefaultArgv (argv: string array) index defValue =
//    if argv.Length-1 < index then defValue
//    else parseOrDefault argv.[index] defValue
//        
//[<EntryPoint>]
//let main argv =
//    let width = parseOrDefaultArgv argv 0 40
//    let height = parseOrDefaultArgv argv 1 30
//    let iterationsToCheck = parseOrDefaultArgv argv 2 20
//
//    let minValue = Complex.Create(-2., -1.);
//    let maxValue = Complex.Create(1., 1.);
//
//    let newLine = System.Environment.NewLine
//
//    let mapPixelToComplex x y =
//        let percentR = (float x) / (float width)
//        let percentI = (float y) / (float height)
//        let r = minValue.RealPart + (maxValue.RealPart - minValue.RealPart) * percentR
//        let i = minValue.ImaginaryPart + (maxValue.ImaginaryPart - minValue.ImaginaryPart) * percentI
//        Complex.Create(r,i)
//
//    let toChar v = match v with
//                   | true -> "X"
//                   | false -> " "
//
//    let inSet c = MandelbrotCalculator.inSet iterationsToCheck c
//
//    let renderRow y =
//        seq{0..width-1}
//        |> Seq.map (fun x -> mapPixelToComplex x y)
//        |> Seq.map inSet 
//        |> Seq.map toChar
//        |> Seq.reduce (+)
//    
//    let stopwatch = System.Diagnostics.Stopwatch.StartNew()
//
//    seq{0..height-1}
//    |> Seq.map renderRow
//    |> Seq.map (fun row -> sprintf "%s%s" row newLine)
//    |> Seq.reduce (+)
//    |> printfn "%s"
//
//    printfn "Width:%d Height:%d Iterations:%d Duration(ms):%d" width height iterationsToCheck stopwatch.ElapsedMilliseconds
//
//    0
//
