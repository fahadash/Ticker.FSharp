using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.FSharp;

namespace Ticker.CSharp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new DataMachine();
            bool initialReceived = false;

            var ticks = machine.GetLiveGoogleApiTicks("AAPL");

            ticks.Subscribe(tick =>
            {
                if (!initialReceived)
                {
                    initialReceived = true;
                    Console.WriteLine("Initial value of Apple stock is {0}", tick.Price);
                }
                else
                {
                    Console.WriteLine("Apple stock price changed to {0}", tick.Price);
                }
            });

            Console.ReadKey();
        }
    }
}
