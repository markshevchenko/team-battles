open System
open System.IO
open Prepare


[<EntryPoint>]
let main = function
    | [| "prepare"; source |] ->
        use source_stream = File.OpenRead source
        print_commands source_stream
        0
    
    | [| "encode" |] ->
        let root = read_huffman_tree Console.In
        let input = Console.ReadLine()
        Encoder.encode root input
        0
        
    | [| "decode" |] ->
        let root = read_huffman_tree Console.In
        let input = Console.ReadLine()
        Decoder.decode root input
        0

    | _ ->
        printfn "huffman prepare <source> | encode | decode"
        1