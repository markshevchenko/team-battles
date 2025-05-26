module proitefest

open System
open System.IO


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
        | Print c -> $"P%c{c}"


let string5 number =
    let value_digits = dict [ (-2, 'A'); (-1, 'B'); (0, '0'); (1, '1'); (2, '2') ]

    let digits =
        seq {
            let mutable tail = number

            while tail > 0 do
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
    static member try_parse = function
        | "simple" -> Some Simple
        | "counter" -> Some Counter
        | "special" -> Some Special
        | _ -> None


type Command = {
    instruction: Instruction
    count: int
}
with
    static member up count = { instruction = Up; count = count }
    
    static member right count = { instruction = Right; count = count }
    
    static member down count = { instruction = Down; count = count }
    
    static member left count = { instruction = Left; count = count }
    
    static member print c count = { instruction = Print(c); count = count }
    
    static member from_instruction count instruction = { instruction = instruction; count = count }
    
    member this.to_string strategy =
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


let convert_ascii_art_to_commands (source: TextReader) =
    let collapse_downs (commands: Command seq) =
        seq {
            use iterator = commands.GetEnumerator()
            if iterator.MoveNext() then
                let mutable last_command = iterator.Current
                
                while iterator.MoveNext() do
                    if last_command.instruction = Down && iterator.Current.instruction = Down then
                        last_command <- Command.down (last_command.count + iterator.Current.count)
                    else
                        yield last_command
                        last_command <- iterator.Current
                        
                if last_command.instruction <> Down then
                    yield last_command
        }
        
    let mutable height = 0
    let mutable width = 0
    let commands =
        seq {
            let mutable line = source.ReadLine()
            let mutable last_column = 0
            
            while line <> null do
                let mutable index = 0
                let mutable is_string_start = true
                
                while index < line.Length do
                    let mutable count = 1
                    if line.[index] = ' ' then
                        while index + count < line.Length && line.[index + count] = ' ' do
                            count <- count + 1
                        
                        if is_string_start then
                            if count = line.Length then
                                yield Command.down 1
                            else if count < last_column then
                                yield Command.left (last_column - count)
                                last_column <- index + count
                            else if count > last_column then
                                yield Command.right (count - last_column)
                                last_column <- index + count
                        else
                            if index + count = line.Length then
                                yield Command.down 1
                            else
                                yield Command.right count
                                last_column <- index + count
                    else 
                        while index + count < line.Length && line.[index + count] = line.[index] do
                            count <- count + 1
                            
                        if is_string_start && last_column <> 0 then
                            yield Command.left last_column
                            last_column <- 0
                            
                        yield Command.print line.[index] count
                        last_column <- last_column + count

                        if last_column = line.Length then
                            last_column <- 0

                    is_string_start <- false
                    index <- index + count

                if width < line.Length then
                    width <- line.Length

                height <- height + 1
                line <- source.ReadLine()
        } |> collapse_downs |> Seq.toArray
    
    (height, width, commands)    


let encode strategy source (target: TextWriter) =
    let height, width, commands = convert_ascii_art_to_commands source

    target.WriteLine height 
    target.WriteLine width
    for command in commands do
        target.Write (command.to_string strategy)
        
        
type DrawMachine(height, width) =
    let mutable canvas = Array2D.create height width ' '
    let mutable row = 0
    let mutable column = 0
    
    let up count =
        row <- row - count
        if row < 0 then row <- 0
        
    let right count =
        column <- column + count
        if column >= width then
            row <- row + column / width
            if row >= height then row <- height - 1
            column <- column % width
            
    let down count =
        row <- row + count
        if row >= height then row <- height - 1
        
    let left count =
        column <- column - count
        if column < 0 then
            row <- row + column / width
            if row < 0 then row <- 0
            column <- (column % width) + width
            
    let print c count =
        for _ in 1..count do
            canvas.[row, column] <- c
            right 1

    member this.execute (command: Command) =
        match command.instruction with
        | Up -> up command.count 
        | Right -> right command.count
        | Down -> down command.count
        | Left -> left command.count
        | Print c -> print c command.count
        
    member this.write (target: TextWriter) =
        for row in 0..height - 1 do
            for column in 0..width - 1 do
                target.Write(canvas.[row, column])
                
            target.WriteLine()


let read_commands (source: TextReader) strategy =
    let mutable last_char = source.Read()
    
    let try_parse_command () =
        let instruction =
            match char last_char with
            | 'U' -> Some Up
            | 'R' -> Some Right
            | 'D' -> Some Down
            | 'L' -> Some Left
            | 'P' ->
                last_char <- source.Read()
                if last_char = -1 then None else Some (Print (char last_char))
            | _ -> None
            
        last_char <- source.Read()
            
        let count =
            if strategy = Counter then
                let mutable result = 0
                while last_char <> -1 && Char.IsAsciiDigit (char last_char) do
                    result <- 10 * result + (last_char - int '0')
                    last_char <- source.Read()
                        
                if result = 0 then 1 else result
            elif strategy = Special then
                let digit_values = dict [ ('A', -2); ('B', -1); ('0', 0); ('1', 1); ('2', 2) ]
                let mutable result = 0
                while last_char <> -1 && digit_values.ContainsKey(char last_char) do
                    result <- 5 * result + digit_values.[char last_char]
                    last_char <- source.Read()

                if result = 0 then 1 else result
            else
                1
                    
        Option.map (Command.from_instruction count) instruction

    seq {
        while last_char <> -1 do
            match (try_parse_command ()) with
            | Some command -> yield command
            | None -> ()
    }


let decode strategy (source: TextReader) target =
    let height = source.ReadLine() |> Int32.Parse
    let width = source.ReadLine() |> Int32.Parse
    let draw_machine = DrawMachine(height, width)
    
    for command in (read_commands source strategy) do
        draw_machine.execute command
    
    draw_machine.write target


let ERROR_NONE = 0
let ERROR_UNPARSED_ARGS = 1
let ERROR_UNKNOWN_STRATEGY = 2
let ERROR_ENCODE = 3
let ERROR_DECODE = 4


[<EntryPoint>]
let main = function
    | [| "encode"; strategy; filename |] ->
        match Strategy.try_parse strategy with
        | Some strategy' ->
            try
                use source = File.OpenText(add_extension filename ".asc")
                use target = File.CreateText(change_extension filename ".dm")
                
                encode strategy' source target

                ERROR_NONE
            with
                | e ->
                    printfn $"Encode error: %s{e.Message}"
                    
                    ERROR_ENCODE
        | None ->
            printfn $"Unknown encode strategy %s{strategy}"
            
            ERROR_UNKNOWN_STRATEGY
    | [| "decode"; strategy; filename |] ->
        match Strategy.try_parse strategy with
        | Some strategy' ->
            try
                use source = File.OpenText(add_extension filename ".dm")
                use target = File.CreateText(change_extension filename ".asc")
                
                decode strategy' source target
                
                ERROR_NONE
            with
                | e ->
                    printfn $"Decode error: %s{e.Message}"

                    ERROR_DECODE
        | None ->
            printfn $"Unknown decode strategy %s{strategy}"
            
            ERROR_UNKNOWN_STRATEGY
    | _ ->
        printfn "proitfest [encode|decode] [simple|counter|special] filename"
        
        ERROR_UNPARSED_ARGS
