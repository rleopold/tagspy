using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TagSpy.Hubs
{
    public class NotifyHub : Hub
    {
        public void NewMediaAvailable(int geographyId)
        {
            Clients.All.newMediaAvailable(geographyId);
        }
    }
}