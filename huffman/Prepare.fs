module Prepare


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

    static member comparer = new NodeComparer()

and NodeComparer() =
    interface IComparer<Node> with
        member this.Compare(x, y) =
            if x.count = y.count
            then StringComparer.InvariantCultureIgnoreCase.Compare(x.id, y.id)
            else int x.count - int y.count



let make_table (stream: Stream) =
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


type Command =
    | Push of byte
    | Combine

    with override this.ToString() =
            match this with
            | Push byte -> $"P{Convert.ToChar(byte)}"
            | Combine -> "C"


let table_to_commands (table: Node array) =
    match table with
    | [||] -> [||]
    | [| Leaf(byte, _) |] -> [| Push byte |]
    | _ ->
        let mutable set = SortedSet(table, Node.comparer)
        let mutable commands = List<Command>()

        let push_if_leaf = function
        | Leaf(byte, _) -> commands.Add(Push byte)
        | Branch _ -> ()

        while set.Count > 1 do
            let right = set.Min
            set.Remove(right) |> ignore

            let left = set.Min
            set.Remove(left) |> ignore

            push_if_leaf left
            push_if_leaf right

            let parent = Node.make_parent left right
            set.Add(parent) |> ignore
            commands.Add(Combine)

        commands |> Seq.toArray


let print_commands (stream: Stream) =
    let table = make_table stream
    let commands = table_to_commands table

    printfn "%d" commands.Length

    for command in commands do
        printfn "%O" command

    stream.Seek(0l, SeekOrigin.Begin) |> ignore
    use reader = new StreamReader(stream, false)

    let mutable line = reader.ReadLine()
    while line <> null do
        printfn "%s" line
        line <- reader.ReadLine()
