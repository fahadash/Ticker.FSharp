open System
open Ticker.FSharp

[<EntryPoint>]
let main argv = 
    let machine = DataMachine()
    let initial_received = ref false

    let ticks = machine.TestTicks("AAPL")

    let received(tick : Tick) =
        if not !initial_received then
            initial_received := true
            printfn "Initial value of Apple stock is %.2f" tick.Price
        else
            printfn "Apple stock value changed to %.2f" tick.Price

    let disposable = ticks.Subscribe received
    Console.ReadKey() |> ignore
    0 // return an integer exit code
