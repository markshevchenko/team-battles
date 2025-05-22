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


let add_extension (file: string) extension =
    if Path.GetExtension(file) = ""
    then Path.ChangeExtension(file, extension)
    else file


[<EntryPoint>]
let main = function
    | [| "encode"; strategy; filename |] ->
        match Strategy.TryParse strategy with
        | Some strategy' ->
            printfn "encode %A %s" strategy' (add_extension filename ".asc")
            0
        | None ->
            printfn "Unknown encode strategy %s" strategy
            1
    | [| "decode"; strategy; filename |] ->
        match Strategy.TryParse strategy with
        | Some strategy' ->
            printfn "decode %A %s" strategy' (add_extension filename ".dm")
            0
        | None ->
            printfn "Unknown decode strategy %s" strategy
            2
    | _ ->
        printfn "proitfest [encode|decode] [simple|counter|special] filename"
        3
