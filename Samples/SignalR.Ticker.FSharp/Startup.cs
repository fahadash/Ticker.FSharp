﻿using Owin;

namespace SignalR.Ticker.FSharp
{
    public static class Startup
    {
        public static void ConfigureSignalR(IAppBuilder app)
        {
            // For more information on how to configure your application using OWIN startup, visit http://go.microsoft.com/fwlink/?LinkID=316888

             app.MapSignalR();
        }
    }
}