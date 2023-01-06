module Mandelbrot.Color

type Color2(r: byte, g: byte, b: byte) =
  member this.R() = r
  member this.G() = g
  member this.B() = b

let Black = new Color2(0uy, 0uy, 0uy)

let namesColors2: Color2[] = 
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
    |> Seq.map (fun (r,g,b) -> new Color2(r |> byte, g |> byte, b |> byte) )
    |> Seq.toArray

let toColor iterations i = 
    let colors = namesColors2 

    match i with
    | i when i < iterations && i > 0 -> colors.[i % colors.Length]
    | _ -> Black
