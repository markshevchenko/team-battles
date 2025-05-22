open System
open System.IO
open System.Text.RegularExpressions


type Instruction =
    | Up
    | Right
    | Down
    | Left
    | Print of char

with
    override this.ToString() =
        match this with
        | Up -> "U"
        | Right -> "R"
        | Down -> "D"
        | Left -> "L"
        | Print c -> sprintf "P%c" c


let string5 number =
    let value_digits = dict [ (-2, 'A'); (-1, 'B'); (0, '0'); (1, '1'); (2, '2') ]

    let digits =
        seq {
            let mutable tail = number

            while tail >= 0 do
                let value =
                    let remainder = tail % 5
                    if remainder > 2 then remainder - 5 else remainder

                tail <- (tail - value) / 5
                yield value_digits.[value] 
        }

    let result = digits |> Seq.rev |> String.Concat
    if result = "" then "0" else result


type Strategy =
    | Simple
    | Counter
    | Special

with
    static member TryParse = function
        | "simple" -> Some Simple
        | "counter" -> Some Counter
        | "special" -> Some Special
        | _ -> None


type Command = {
    instruction: Instruction
    count: int
}

with
    member this.ToString strategy =
        match strategy, this.count with
        | Simple, count' -> String.replicate count' (string this.instruction)
        | _, 1 -> (string this.instruction)
        | Counter, count' -> (string this.instruction) + (string count')
        | Special, count' -> (string this.instruction) + (string5 count')


let change_extension file extension =
    Path.ChangeExtension(file, extension)


let add_extension (file: string) extension =
    if Path.GetExtension(file) = ""
    then change_extension file extension
    else file


type Block =
    | Spaces of int
    | Printable of string


let source_to_block_lines (source: TextReader) =
    seq {
        let mutable line = source.ReadLine()
        while line <> null do
            yield
                if line = "" || Seq.forall ((=) ' ') line
                then
                    [| |]
                else
                    Regex.Split(line, "\\b")
                    |> Seq.filter ((<>) "")
                    |> Seq.map (fun s -> if s.[0] = ' ' then Spaces(s.Length) else Printable(s))
                    |> Seq.toArray

            line <- source.ReadLine()
    }


let parse_block_line (block_line: Block array) previous_column =
    let new_column, start =
        match block_line.[0] with
        | Spaces count -> count, 1
        | _ -> 0, 0
    
    let finish =
        match block_line.[block_line.Length - 1] with
        | Spaces _ -> block_line.Length - 2
        | _ -> block_line.Length - 1

    let mutable current_column = new_column
    let commands =
        seq {
            if new_column > previous_column then
                yield { instruction = Right; count = new_column - previous_column }
            else if new_column < previous_column then 
                yield { instruction = Left; count = previous_column - new_column }

            for i = start to finish do
                match block_line.[i] with
                | Spaces count ->
                    yield { instruction = Right; count = count }
                    current_column <- current_column + count
                | Printable chars ->
                    let mutable last_index = 0
                    let mutable count = 1
                   
                    while last_index + count <= chars.Length do
                        while last_index + count < chars.Length && chars.[last_index + count] = chars.[last_index] do
                            count <- count + 1

                        current_column <- current_column + count
                        last_index <- last_index + count

                        yield { instruction = Print(chars.[last_index]); count = count }
        }

    (current_column, commands)


let encode strategy source (target: TextWriter) =
    let block_lines =
        source_to_block_lines source
        |> Seq.rev
        |> Seq.skipWhile ((=) [| |])
        |> Seq.rev

    let mutable column = 0
    for block_line in block_lines do
        let new_column, commands = parse_block_line block_line column

        for command in commands do
            target.Write(command.ToString strategy)

        column <- new_column


let decode strategy source target =
    ()


let ERROR_NONE = 0
let ERROR_UNPARSED_ARGS = 1
let ERROR_UNKNOWN_STRATEGY = 2
let ERROR_ENCODE = 3
let ERROR_DECODE = 4


[<EntryPoint>]
let main = function
    | [| "encode"; strategy; filename |] ->
        match Strategy.TryParse strategy with
        | Some strategy' ->
            try
                use source = File.OpenText(add_extension filename ".asc")
                use target = File.CreateText(change_extension filename ".dm")
                
                encode strategy' source target

                ERROR_NONE
            with
                | e ->
                    printfn "Encode error: %s" e.Message
                    
                    ERROR_ENCODE
        | None ->
            printfn "Unknown encode strategy %s" strategy
            
            ERROR_UNKNOWN_STRATEGY
    | [| "decode"; strategy; filename |] ->
        match Strategy.TryParse strategy with
        | Some strategy' ->
            try
                use source = File.OpenText(add_extension filename ".dm")
                use target = File.CreateText(change_extension filename ".asc")
                
                decode strategy' source target
                
                ERROR_NONE
            with
                | e ->
                    printfn "Decode error: %s" e.Message

                    ERROR_DECODE
        | None ->
            printfn "Unknown decode strategy %s" strategy
            
            ERROR_UNKNOWN_STRATEGY
    | _ ->
        printfn "proitfest [encode|decode] [simple|counter|special] filename"
        
        ERROR_UNPARSED_ARGS
