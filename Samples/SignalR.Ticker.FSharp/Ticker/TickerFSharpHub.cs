using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Ticker.FSharp.Ticker
{
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    using global::Ticker.FSharp;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    public class TickWithIndicator
    {
        public Tick Tick { get; set; }
        public bool Up { get; set; }
    }

    [HubName("tickerFsharp")]
    public class TickerFSharpHub : Hub
    {

        private static IDisposable subscription = null;

        [HubMethodName("startTicker")]
        public void StartTicker()
        {
            if (subscription == null)
            {
                var machine = new DataMachine();
                var aapl = this.AddIndicator(machine.GetLiveGoogleApiTicks("AAPL"));
                var goog = this.AddIndicator(machine.GetLiveGoogleApiTicks("GOOG"));
                var msft = this.AddIndicator(machine.GetLiveGoogleApiTicks("MSFT"));


                subscription = aapl
                    .Merge(msft)
                    .Merge(goog)
                    .Subscribe(
                    t =>
                        {
                            this.Tick(t.Tick.Symbol, t.Tick.Price, t.Tick.LastUpdated.ToString(), t.Up);
                        });
            }
        }
        
        public void Tick(string symbol, double price, string lastUpdated, bool up)
        {
            Clients.All.tick(symbol, price.ToString("###0.00"), lastUpdated, up);
        }

        IObservable<TickWithIndicator> AddIndicator(IObservable<Tick> ticker)
        {
            return ticker.FirstAsync().Concat(ticker.Skip(1))
                    .Select(t => new TickWithIndicator()
                    {
                        Tick = t,
                        Up = true
                    })
                    .Scan(
                    (acc, n) =>
                    (acc == null) ? n : new TickWithIndicator() { Tick = n.Tick, Up = acc.Tick.Price < n.Tick.Price });
        }
    }
}