module ColorModule
open System.Drawing

let toColor i = 
  let r = i % 256
  let g = (i / 256) % 256
  let b = (i / (256 * 256)) % 256
  Color.FromArgb(r,g,b)
