open System
open System.Text


let code_char shift c =
    let letter_count = 'z'B - 'a'B + 1uy
    let shift26 = shift % letter_count |> byte
    if c >= 'a'B && c <= 'z'B
    then (c - 'a'B + shift26) % letter_count + 'a'B
    elif c >= 'A'B && c <= 'Z'B
    then (c - 'A'B + shift26) % letter_count + 'A'B
    else c
                        

let encode shift (source: string) =
    source
    |> Encoding.ASCII.GetBytes
    |> Array.map (code_char shift)
    |> Encoding.ASCII.GetString


let decode shift (source: string) =
    source
    |> Encoding.ASCII.GetBytes
    |> Array.map (code_char (104uy - shift))
    |> Encoding.ASCII.GetString


[<EntryPoint>]
let main = function
    | [| "encode" |] ->
        let shift = Console.ReadLine() |> byte
        let source = Console.ReadLine()
        printfn "%s" (encode shift source)
        
        0
    | [| "decode" |] ->
        let shift = Console.ReadLine() |> byte
        let source = Console.ReadLine()
        printfn "%s" (decode shift source)
        
        0
    | _ ->
        printfn "caesar encode | decode"
        
        1
