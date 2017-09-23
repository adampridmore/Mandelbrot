module Mandelbrot.Color

open System.Drawing

let private namesColors2 = 
    [|
        (66, 30, 15);
        (25, 7, 26);
        (9, 1, 47);
        (4, 4, 73);
        (0, 7, 100);
        (12, 44, 138);
        (24, 82, 177);
        (57, 125, 209);
        (134, 181, 229);
        (211, 236, 248);
        (241, 233, 191);
        (248, 201, 95);
        (255, 170, 0);
        (204, 128, 0);
        (153, 87, 0);
        (106, 52, 3);
    |]
    |> Seq.map (fun (r,g,b) -> Color.FromArgb(255, r, g, b) )
    |> Seq.toArray

let toColor iterations i = 
//    let colors = [|
//        Color.Blue;
//        Color.Red;
//        Color.Green;
//        Color.Yellow;
//        Color.Orange;
//        Color.BlueViolet;
//        Color.Ivory
//    |]

//    let namesBluesColors = 
//        [|
//            Color.AliceBlue;
//            Color.Blue;
//            Color.BlueViolet;
//            Color.CadetBlue;
//            Color.CornflowerBlue;
//            Color.DarkBlue;
//            Color.DarkSlateBlue;
//            Color.DeepSkyBlue
//            Color.DodgerBlue;
//            Color.LightBlue;
//            Color.LightSkyBlue;
//            Color.LightSteelBlue;
//            Color.MediumBlue;
//            Color.MediumSlateBlue;
//            Color.MidnightBlue;
//            Color.PowderBlue;
//            Color.RoyalBlue;
//            Color.SkyBlue;
//            Color.SlateBlue;
//            Color.SteelBlue
//        |]
//        |> Seq.sortBy (fun c -> c.B + c.G + c.R)
//        |> Seq.toArray




//        |> Seq.rev


//    let allBlues =
//        seq{0..255}
//        |> Seq.map (fun i -> Color.FromArgb(150, 150, i))
//        |> Seq.toArray
        
    let colors = namesColors2 

    //colors.[i % colors.Length]

    match i with
    | i when i < iterations && i > 0 -> colors.[i % colors.Length]
    | _ -> Color.Black