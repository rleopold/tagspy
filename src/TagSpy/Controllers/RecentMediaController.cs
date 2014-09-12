using InstaSharp;
using InstaSharp.Endpoints;
using InstaSharp.Models;
using InstaSharp.Models.Responses;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TagSpy.Data;
using TagSpy.Models;
using WindowsAzure.Table;

namespace TagSpy.Controllers
{
    [Authorize]
    public class RecentMediaController : ApiController
    {
        private Geographies _geoEndpoint;
        private Tags _tagEndpoint;
        private ILogger _log;
        private InstagramConfig _config;
        private ISubscriptionRepository _subscriptions;

        public RecentMediaController(ILogger log, InstagramConfig config, ISubscriptionRepository subscriptions)
        {
            _geoEndpoint = new Geographies(config);
            _tagEndpoint = new Tags(config);
            _config = config;
            _log = log;
            _subscriptions = subscriptions;
        }

        public async Task<IHttpActionResult> Get(string type, string object_id)
        {
            MediasResponse response;
            if(type == "place")
            {
                _log.Information("request for RECENT GEO media for object: {0}", object_id);
                response = await _geoEndpoint.Recent(int.Parse(object_id));
            }
            else
            {
                _log.Information("request for RECENT TAG media for object: {0}", object_id);
                response = await _tagEndpoint.Recent(object_id);
            }
            
            return Ok(response);
        }

        public async Task<IHttpActionResult> Get(string type, string object_id, string minMediaId)
        {
            MediasResponse response;
            if(_subscriptions.HasMinMediaBeenServed(User.Identity.Name, object_id, minMediaId))
            {
                _log.Warning("SUNK REQUEST {0} starting at id: {1}", object_id, minMediaId);
                return Ok();
            }
            if (type == "place")
            {
                _log.Information("request for LATEST GEO media for object: {0} starting at id: {1}", object_id, minMediaId);
                response = await _geoEndpoint.Recent(int.Parse(object_id), null, minMediaId);
                if (response.Data.Count > 0) //we served something
                    _subscriptions.MarkMinMediaServed(User.Identity.Name, object_id, minMediaId);
            }
            else
            {
                _log.Information("request for LATEST TAG media for object: {0} starting at id: {1}", object_id, minMediaId);
                response = await _tagEndpoint.Recent(object_id, minMediaId, null, null);
                if (response.Data.Count > 0) //we served something
                    _subscriptions.MarkMinMediaServed(User.Identity.Name, object_id, minMediaId);
            }
            return Ok(response);
        }

    }
}
