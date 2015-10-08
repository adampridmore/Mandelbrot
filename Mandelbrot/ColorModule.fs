module ColorModule
open System.Drawing

let toColor i = 
    let colors = [|
        Color.Blue;
        Color.Red;
        Color.Green;
        Color.Yellow;
        Color.Orange;
        Color.BlueViolet;
        Color.Ivory
    |]
    colors.[i%colors.Length]

