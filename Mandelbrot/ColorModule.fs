module ColorModule
open System.Drawing

let toColor i = 
//    let colors = [|
//        Color.Blue;
//        Color.Red;
//        Color.Green;
//        Color.Yellow;
//        Color.Orange;
//        Color.BlueViolet;
//        Color.Ivory
//    |]

    let namesBluesColors = 
        [|
            Color.AliceBlue;
            Color.Blue;
            Color.BlueViolet;
            Color.CadetBlue;
            Color.CornflowerBlue;
            Color.DarkBlue;
            Color.DarkSlateBlue;
            Color.DeepSkyBlue
            Color.DodgerBlue;
            Color.LightBlue;
            Color.LightSkyBlue;
            Color.LightSteelBlue;
            Color.MediumBlue;
            Color.MediumSlateBlue;
            Color.MidnightBlue;
            Color.PowderBlue;
            Color.RoyalBlue;
            Color.SkyBlue;
            Color.SlateBlue;
            Color.SteelBlue
        |]
        |> Seq.sortBy (fun c -> c.B + c.G + c.R)
        |> Seq.toArray


//    let allBlues =  
//        seq{0..25..255}
//        |> Seq.map (fun i -> Color.FromArgb(255, 255, i))
//        |> Seq.toArray
        
    let colors = namesBluesColors

    colors.[i%colors.Length]

