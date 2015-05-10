using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.Ticker.FSharp
{
    using Owin;

    public class MyStartup
    {
        public void Configuration(IAppBuilder app)
        {
            Startup.ConfigureSignalR(app);
        }
    }
}