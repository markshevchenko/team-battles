open System
open System.IO
open Prepare


[<EntryPoint>]
let main args =
    // match args with
    // | [| "prepare"; source |] ->
    //     use source_stream = File.OpenRead source
    //     print_commands source_stream
    //     0
    //
    // | [| "encode"; table |] ->
    //     let root = read_huffman_tree table
    //     use input_stream = Console.OpenStandardInput()
    //     use output_stream = Console.OpenStandardOutput()
    //     Encoder.encode root input_stream output_stream
    //     0
        
    // | [| "decode"; table |] ->
        // let root = read_huffman_tree table
        let root = read_huffman_tree "abc_huffman.txt"
        // use input_stream = Console.OpenStandardInput()
        use input_stream = File.OpenRead "abc_encoded.txt"
        use output_stream = Console.OpenStandardOutput()
        Decoder.decode root input_stream output_stream
        0

    // | _ ->
    //     printfn "huffman prepare <source> | encode <table> | decode <table>"
    //     1