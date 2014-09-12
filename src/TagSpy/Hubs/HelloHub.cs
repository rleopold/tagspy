using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TagSpy.Hubs
{
    public class HelloHub : Hub
    {
        //TODO: this is just signalr test code, remove
        public void Hello(string name)
        {
            Clients.All.hello(string.Format("hello {0}, the time is {1}", name, DateTime.Now.ToString()));
        }
    }
}