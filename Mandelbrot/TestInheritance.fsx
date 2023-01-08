[<AbstractClass>]
type BaseClass(param1) =

    abstract member Param3: string -> string

    member this.Param1 = param1

    member private this.MyMethod() = param1()


type DerivedClassA(param1, param2) =
    inherit BaseClass(param1)
    member this.Param2 = param2
    override this.Param3(s: string) : string = s

// test
let derived = new DerivedClassA(1,2)
printfn "param1=%O" derived.Param1
printfn "param2=%O" derived.Param2


