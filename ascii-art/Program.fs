open Decoder
open Encoder

[<EntryPoint>]
let main = function
    | [|"decode"|] -> Decoder.decode (); 0
    | [|"encode"|] -> Encoder.encode (); 0
    | _ -> printfn "ascii-arg decode | encode"; 1
