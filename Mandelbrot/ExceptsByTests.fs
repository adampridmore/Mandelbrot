module ExceptsByTests

type MyType = {
    Name: string;
    I: int
}

let l1 = [
        {Name= "A";I=1};
        {Name= "B";I=2}
    ]

let l2 = [
    {Name= "B";I=3};
    {Name= "C";I=4}
]


// exceptBy l1 l2 (fun x -> x.Name) (fun x -> x.Name)