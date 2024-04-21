open System
open System.IO
open Prepare


[<EntryPoint>]
let main = function
    | [| "prepare"; filename |] ->
        use input_stream = File.OpenRead filename
        Prepare.print_commands input_stream

        0
    | _ ->
        printfn "huffman prepare <filename> | encode | decode"

        1