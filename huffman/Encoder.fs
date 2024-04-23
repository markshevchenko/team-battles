module Encoder

open System
open System.Collections.Generic
open System.IO
open System.Text
open Huffman


let prepare_codes (root: Node) =
    let mutable string_builder = StringBuilder()
    let mutable codes = Dictionary<int, byte[]>()
    let rec visit = function
        | Leaf(byte, _) -> codes.[int byte] <- Encoding.ASCII.GetBytes(string_builder.ToString())
        | Branch(left, right, _, _) ->
            string_builder.Append('0') |> ignore
            visit left
            string_builder.Length <- string_builder.Length - 1
            
            string_builder.Append('1') |> ignore
            visit right
            string_builder.Length <- string_builder.Length - 1
            
    visit root
    codes


let encode (root: Node) (input_stream: Stream) (output_stream: Stream) =
    let codes = prepare_codes root
    
    let mutable next_byte = input_stream.ReadByte()
    while next_byte <> -1 do
        output_stream.Write(codes.[next_byte])
        next_byte <- input_stream.ReadByte()
