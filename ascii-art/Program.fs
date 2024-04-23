open System
open System.IO

let input (reader: TextReader) =
    seq {
    let mutable line = reader.ReadLine()
    while line <> null do
        yield line
        line <- reader.ReadLine()
    } |> Seq.toArray


[<EntryPoint>]
let main = function
    | [| "decode" |] -> Console.In |> input |> Decoder.decode; 0
    | [| "encode" |] -> Console.In |> input |> Encoder.encode; 0
    | _ -> printfn "ascii-art decode | encode"; 1
