module Decoder

open System
open Huffman


let decode (root: Node) (input: string) =
    let rec parse_bit = function
        | Leaf(byte, _), [] ->
            printf "%O" (byte |> Convert.ToChar)
        | Leaf(byte, _), cs ->
            printf "%O" (byte |> Convert.ToChar)
            parse_bit(root, cs)
        | Branch(left, _, _, _), '0'::cs ->
            parse_bit(left, cs)
        | Branch(_, right, _, _), '1'::cs ->
            parse_bit(right, cs)
        | _ -> failwith "Invalid input"
    
    parse_bit(root, input |> Seq.toList)
