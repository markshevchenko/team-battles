open System

let digit_values = dict [ ('0', 0); ('1', 1); ('2', 2); ('3', 3); ('4', 4)
                          ('a', -1); ('b', -2); ('c', -3); ('d', -4); ('e', -5) ]

let value_digits = dict [ (0, '0'); (1, '1'); (2, '2'); (3, '3'); (4, '4')
                          (-1, 'a'); (-2, 'b'); (-3, 'c'); (-4, 'd'); (-5, 'e') ]


let decode (digits: char seq) =
    let mutable result = 0
    
    for digit in digits do
        let value = digit_values[digit]
        result <- 10 * result + value

    result
    
    
let encode (number: int32) =
    let digits =
        seq {
            let mutable tail = number
            
            while tail > 0 do
                let mutable value = tail % 10
                if value > 4 then value <- value - 10
                tail <- (tail - value) / 10
                yield value_digits[value]
        }
        
    let result = digits |> Seq.rev |> String.Concat
    if result = "" then "0" else result


[<EntryPoint>]
let main = function
    | [| "encode" |] ->
        let number = Console.ReadLine() |> int32
        printfn "%s" (encode number)
        
        0
    | [| "decode" |] ->
        let digits = Console.ReadLine()
        printfn "%d" (decode digits)
        
        0
    | _ ->
        printfn "number-system encode | decode"
        
        1