namespace Ticker.FSharp

open System
open Newtonsoft.Json

type Tick() = 
        [<JsonProperty(PropertyName = "t")>] 
        member val Symbol = Unchecked.defaultof<string> with get, set

        [<JsonProperty(PropertyName = "l")>]
        member val Price = Unchecked.defaultof<float> with get, set

        [<JsonProperty(PropertyName = "lt_dts")>]
        member val LastUpdated = Unchecked.defaultof<DateTime> with get, set

