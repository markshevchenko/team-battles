open System
open System.Text

type Command =
    | Up of int
    | Right of int
    | Down of int
    | Left of int
    | Print of char


let parse_c (c: char) cs =
    match cs with
    | c2::cs2 when c = c2 -> Some(c2, cs2)
    | _ -> None
    
    
let parse_any cs =
    match cs with
    | c2::cs2 -> Some(c2, cs2)
    | [] -> None
    
    
let digit (cs: char list) =
    match cs with
    | c2::cs2 when Char.IsAsciiDigit(c2) -> Some(c2, cs2)
    | _ -> None
    
    
let rec many p (cs: char list) =
    match p cs with
    | Some(head, cs2) ->
        match many p cs2 with
        | Some(tail, cs3) -> Some(head::tail, cs3)
        | None -> Some([head], cs2)
    | None -> Some([], cs)


let (||>) p f (cs: char list) =
    match p cs with
    | Some(value, cs2) -> Some(f value, cs2)
    | None -> None
    
    
let (>>.) p1 p2 (cs: char list) =
    match p1 cs with
    | Some(_, cs2) -> p2 cs2
    | None -> None
    
    
let (<|>) p1 p2 (cs: char list) =
    match p1 cs with
    | Some(value, cs2) -> Some(value, cs2)
    | None -> p2 cs


let make_digits (digits: char list) =
    let number = String.Concat(Array.ofList digits)
    if number = ""
    then 1
    else int number

let parse_direct_command letter =
    parse_c letter >>. (many digit) ||> make_digits
    
let parse_command =
    (parse_direct_command 'U' ||> Up)
    <|> (parse_direct_command 'R' ||> Right)
    <|> (parse_direct_command 'D' ||> Down)
    <|> (parse_direct_command 'L' ||> Left)
    <|> (parse_c 'P' >>. parse_any ||> Print) 
    
let height = Console.ReadLine() |> int
let width = Console.ReadLine() |> int
let input = Console.ReadLine() |> List.ofSeq
let commands = many parse_command input |> Option.map fst |> Option.defaultValue []

let mutable screen =
    let line = String.replicate width " "
    [| for _ in 1..height -> StringBuilder(line) |]
let mutable row = 0
let mutable column = 0

for command in commands do
    match command with
    | Up count -> row <- row - count
    | Right count -> column <- column + count
    | Down count -> row <- row + count
    | Left count -> column <- column - count
    | Print c -> screen.[row].[column] <- c

for line in screen do
    printfn $"%s{string line}"