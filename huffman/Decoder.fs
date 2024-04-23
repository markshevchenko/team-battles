module Decoder

open System.IO
open Huffman


let decode (root: Node) (input_stream: Stream) (output_stream: Stream) =
    let read_bit (stream: Stream) =
        let next_byte = stream.ReadByte() 
        if next_byte = int '0'B
        then Some false
        elif next_byte = int '1'B
        then Some true
        else None
        
    let rec parse_bit = function
        | Leaf(byte, _), None ->
            output_stream.WriteByte(byte)
        | Leaf(byte, _), next_bit ->
            output_stream.WriteByte(byte)
            parse_bit(root, next_bit)
        | Branch(left, _, _, _), Some false ->
            parse_bit(left, read_bit input_stream)
        | Branch(_, right, _, _), Some true ->
            parse_bit(right, read_bit input_stream)
        | _ -> failwith "Invalid input"
    
    parse_bit(root, read_bit input_stream)
