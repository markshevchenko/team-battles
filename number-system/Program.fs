open System

let digit_values = dict [ ('0', 0L); ('1', 1L); ('2', 2L); ('3', 3L); ('4', 4L)
                          ('a', -1L); ('b', -2L); ('c', -3L); ('d', -4L); ('e', -5L) ]

let value_digits = dict [ (0L, '0'); (1L, '1'); (2L, '2'); (3L, '3'); (4L, '4')
                          (-1L, 'a'); (-2L, 'b'); (-3L, 'c'); (-4L, 'd'); (-5L, 'e') ]


let decode (digits: char seq) =
    let mutable result = 0L
    
    for digit in digits do
        let value = digit_values[digit]
        result <- 10L * result + value

    result
    
    
let encode (number: int64) =
    let digits =
        seq {
            let mutable tail = number
            
            while tail > 0 do
                let mutable value = tail % 10L
                if value > 4 then value <- value - 10L
                tail <- (tail - value) / 10L
                yield value_digits[value]
        }
        
    digits |> Seq.rev |> String.Concat


[<EntryPoint>]
let main = function
    | [| "encode" |] ->
        let number = Console.ReadLine() |> int64
        printfn "%s" (encode number)
        
        0
    | [| "decode" |] ->
        let digits = Console.ReadLine()
        printfn "%d" (decode digits)
        
        0
    | _ ->
        printfn "number-system encode | decode"
        
        1