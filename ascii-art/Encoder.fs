module Encoder

open DrawMachine

let private make_commands (rows: string array) =
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
    
let public encode (input: string array) =
    printfn $"%d{input.Length}"
    printfn $"%d{input |> Seq.map (fun s -> s.Length) |> Seq.max}"

    input
    |> make_commands
    |> Seq.map (fun command -> command.ToString())
    |> Seq.iter (printf "%s")
