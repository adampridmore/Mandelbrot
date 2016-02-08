module ExceptsBy

let existsBy value fn seq =
    seq
    |> Seq.where (fun x -> fn x = value) 
    |> Seq.isEmpty 
    |> not

let exceptBy l1 l2 fn1 fn2 =
    l2 
    |> Seq.filter (fun v2 -> existsBy (fn2 v2) fn1 l1 |> not)
    




