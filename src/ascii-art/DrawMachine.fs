module DrawMachine

type Command =
    | Up of uint
    | Right of uint
    | Down of uint
    | Left of uint
    | Print of char
    
    with override this.ToString() =
            match this with
            | Up 1u -> "U"
            | Up count -> "U" + (string count)
            | Right 1u -> "R"
            | Right count -> "R" + (string count)
            | Down 1u -> "D"
            | Down count -> "D" + (string count)
            | Left 1u -> "L"
            | Left count -> "L" + (string count)
            | Print symbol -> "P" + (string symbol)

