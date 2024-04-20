module Encoder

open System

type Command =
    | Up of uint
    | Right of uint
    | Down of uint
    | Left of uint
    | Print of char
    
let commandToString = function
    | Up 1u -> "U"
    | Up count -> "U" + (string count)
    | Right 1u -> "R"
    | Right count -> "R" + (string count)
    | Down 1u -> "D"
    | Down count -> "D" + (string count)
    | Left 1u -> "L"
    | Left count -> "L" + (string count)
    | Print symbol -> "P" + (string symbol)
    
let makeCommands (rows: string array) =
    seq {
        for row in 0..rows.Length - 1 do
            let mutable skip_length = 0u
            for c in rows.[row] do
                if c = ' '
                then
                    skip_length <- skip_length + 1u
                else
                    yield Right(skip_length)
                    yield Print(c)
                    skip_length <- 1u
                        
            if row < rows.Length - 1
            then
                if skip_length > 0u then
                    yield Left(uint rows.[row].Length - skip_length)
                yield Down(1u)
    }
    
let encode () =
    let input =
        seq {
            let mutable line = Console.ReadLine()
            while line <> null do
                yield line
                line <- Console.ReadLine()
        } |> Seq.toArray

    printfn $"%d{input.Length}"
    printfn $"%d{input |> Seq.map (fun line -> line.Length) |> Seq.max}"

    let commands = makeCommands input

    commands
    |> Seq.iter (commandToString >> printf "%s")
