module Prepare

open System
open System.Collections.Generic
open System.IO
open Huffman


type Command =
    | Push of byte
    | Combine
            
    with override this.ToString() =
            match this with
            | Push byte -> $"P{Convert.ToChar(byte)}"
            | Combine -> "C"

            static member parse = function
                    | "C" -> Combine
                    | s when s.[0] = 'P' -> Push (byte s.[1])
                    | _ -> failwith "Invalid Command"


let huffman_tree_to_commands (root: Node option) =
    match root with
    | None -> [| |]
    | Some (Leaf(byte, _)) -> [| Push byte |]
    | Some (Branch _ as root) -> 
        let rec generate_commands accumulator = function
            | Leaf(byte, _) -> Push byte::accumulator
            | Branch(left, right, _, _) ->
                let right_subtree = generate_commands accumulator right
                let tree = generate_commands right_subtree left 
                Combine::tree
                
        generate_commands [] root |> List.rev |> List.toArray


let commands_to_huffman_tree (commands: Command seq) =
    let mutable node_stack = Stack<Node>()
    for command in commands do
        let node =
            match command with
            | Push byte ->
                Leaf(byte, 0ul)
            | Combine ->
                let right = node_stack.Pop()
                let left = node_stack.Pop()
                Branch(left, right, String.Empty, 0ul)
                
        node_stack.Push(node)

    node_stack.Pop()


let read_huffman_tree (reader: TextReader) =
    let count = reader.ReadLine() |> int
    let commands = seq {
        for i in 1..count do
            yield Command.parse (reader.ReadLine())
    }

    commands_to_huffman_tree commands    


let print_commands (stream: Stream) =
    let commands =
        stream
        |> count_bytes
        |> make_huffman_tree
        |> huffman_tree_to_commands
        
    printfn "%d" commands.Length
    Array.iter (printfn "%O") commands
