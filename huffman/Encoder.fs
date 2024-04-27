module Encoder

open System
open System.Collections.Generic
open System.Text
open Huffman


let prepare_codes (root: Node) =
    let mutable string_builder = StringBuilder()
    let mutable codes = Dictionary<char, string>()
    let rec visit = function
        | Leaf(byte, _) -> codes.[byte |> Convert.ToChar] <- string_builder.ToString()
        | Branch(left, right, _, _) ->
            string_builder.Append('0') |> ignore
            visit left
            string_builder.Length <- string_builder.Length - 1
            
            string_builder.Append('1') |> ignore
            visit right
            string_builder.Length <- string_builder.Length - 1
            
    visit root
    codes


let encode (root: Node) (input: string) =
    let codes = prepare_codes root
    
    for c in input do
        printf "%s" codes.[c]
