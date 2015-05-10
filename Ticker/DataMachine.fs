namespace Ticker.FSharp

open System
open System.IO
open System.Net
open System.Reactive
open System.Reactive.Disposables
open System.Reactive.Linq
open System.Text
open System.Threading
open System.Threading.Tasks

open Newtonsoft.Json

type DataMachine() = 
    member this.GetLiveGoogleApiTicks(symbol : string) =
        //google_api queries Google APi every second and produces the string output
        let google_api (symbol : string) =
            // When operation is cancelled by the caller, this variable will be modified
            let cancelled =  ref false
            let operation(o : IObserver<string>) =
                //Time to wait before fetching again through the API
                let timeToSleep = TimeSpan.FromSeconds(1.0)
                //Google Finance API , notice q is the query string parameter for symbol
                let urlBuilder = new StringBuilder("""http://www.google.com/finance/info?q=""")
                urlBuilder.Append symbol
                let url = urlBuilder.ToString()
                let client = new WebClient()
                let cancelToken = Task.Factory.CancellationToken
                client.BaseAddress = url
                let task() =
                            try
                                while not cancelToken.IsCancellationRequested && not !cancelled do
                                    let data = client.DownloadString url
                                   //Produce the item in observable
                                    o.OnNext data
                                    Thread.Sleep timeToSleep
                                    while client.IsBusy do
                                        Thread.Sleep 100
                            finally
                                    ignore

                Task.Factory.StartNew task         
                let a = new Action(fun () -> (cancelled := true))
                a |> Disposable.Create

            Observable.Create(operation)

        let strings = (google_api symbol)
        // Eliminate unparseable characters for Json.Net library
        let trimmed =  strings.Select (fun (l:string) -> l.Substring(l.IndexOf("[") + 1, l.LastIndexOf("]") - 2 - l.IndexOf("[") + 1))
        //Parse into object and return the IObservable<Tick>
        trimmed.Select(fun (l:string) -> JsonConvert.DeserializeObject<Tick>(l)).DistinctUntilChanged(fun (a:Tick) -> a.Price)

    member this.TestTicks(symbol : string) =
        let random = Random()
        Observable.Interval(TimeSpan.FromSeconds(2.0)).Select(fun i -> Tick(Symbol=symbol, Price=random.NextDouble(), LastUpdated=DateTime.Now))