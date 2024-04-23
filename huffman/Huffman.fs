module Huffman

open System
open System.Collections.Generic
open System.IO


type Node =
    | Branch of Node * Node * string * uint32
    | Leaf of byte * uint32

    member this.count =
        match this with
        | Branch(_, _, _, count) -> count
        | Leaf(_, count) -> count

    member this.id =
        match this with
        | Branch(_, _, id, _) -> id
        | Leaf(byte, _) -> byte |> Convert.ToChar |> string

    static member make_parent left right =
        Branch(left, right, left.id + right.id, left.count + right.count)

    static member make_leaf byte count =
        Leaf(byte, count)
    static member comparer = NodeComparer()
and NodeComparer() =
    interface IComparer<Node> with
        member this.Compare(x, y) =
            if x.count = y.count
            then StringComparer.InvariantCultureIgnoreCase.Compare(y.id, x.id)
            else int x.count - int y.count


let count_bytes (stream: Stream) =
    let mutable counts = Array.create 256 0ul

    let mutable next_byte = stream.ReadByte()
    let EOF = -1
    while next_byte <> EOF do
        counts.[next_byte] <- counts.[next_byte] + 1ul
        next_byte <- stream.ReadByte()

    counts
    |> Seq.mapi (fun b -> Node.make_leaf (byte b))
    |> Seq.filter (fun node -> node.count > 0ul)
    |> Seq.toArray


let make_huffman_tree (table: Node array) =
    match table with
    | [||] -> None
    | [| Leaf(byte, count) |] -> Some(Leaf(byte, count))
    | _ ->
        let mutable set = SortedSet(table, Node.comparer)

        while set.Count > 1 do
            let right = set.Min
            set.Remove(right) |> ignore

            let left = set.Min
            set.Remove(left) |> ignore

            let parent = Node.make_parent left right
            set.Add(parent) |> ignore

        Some(set |> Seq.head)
