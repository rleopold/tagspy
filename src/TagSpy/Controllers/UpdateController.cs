using InstaSharp;
using InstaSharp.Endpoints;
using InstaSharp.Models;
using Microsoft.AspNet.SignalR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TagSpy.Hubs;

namespace TagSpy.Controllers
{
    public class UpdateController : ApiController
    {
        private IHubContext _notificationContext;
        private ILogger _log;
        private InstagramConfig _config;

        public UpdateController(InstagramConfig config, ILogger log)
        {
            _config = config;
            _log = log;
            _notificationContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
        }
        /// <summary>
        /// Recieves the challenge from InstaGram for creating a new subscription
        /// </summary>
        /// <param name="hub"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get([FromUri]hub hub)
        {
            //TODO: parse mode and verify token
            string result = hub.challenge;
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(result, Encoding.UTF8, "text/plain");

            _log.Information("received challenge: {0}", hub.challenge);
            return resp;
        }

        /// <summary>
        /// recieves real-time update notices from Instagram
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post(Realtime[] content)
        {
            var geoEndpoint = new Geographies(_config);

            //TODO: parse content and broadcast to clients
            for (int i = 0; i < content.Length; i++)
            {
                _log.Information("content push recieved: {0}: {1}",  content[i].Object, content[i].Object_ID);
                _notificationContext.Clients.All.newMediaAvailable(content[i].Object_ID);
            }

            return Ok();
        }

         
    }
}
