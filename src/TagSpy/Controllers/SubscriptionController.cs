using InstaSharp;
using InstaSharp.Endpoints;
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
    public class SubscriptionController : ApiController
    {
        private ILogger _log;
        private InstagramConfig _config;
        private Subscription _subscriptionEndpoint;
        private ISubscriptionRepository _subscriptions;

        public SubscriptionController(ISubscriptionRepository subscriptions, ILogger log, InstagramConfig config)
        {
            _log = log;
            _config = config;
            _subscriptionEndpoint = new Subscription(_config);
            _subscriptions = subscriptions;
        }

        public async Task<IHttpActionResult> Get()
        {
            var response = await _subscriptions.GetSubscriptionListByUserAsync(User.Identity.Name);
            _log.Information("Request List Subscriptions");

            return Ok(response);
        }

        /// <summary>
        /// Returns subscriptions for the current user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type">either "tag" or "place"</param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Get(string type)
        {
            var user = User.Identity.Name;
            switch (type)
            {
                case "tag":
                    return Ok(await _subscriptions.GetTagSubscriptionsByUserAsync(user));
                case "place":
                    return Ok( await _subscriptions.GetPlaceSubscriptionsByUserAsync(user));
                default:
                    return NotFound();
            }
        }


        public async Task<IHttpActionResult> Post(CreateSub sub)
        {
            SubscriptionResponse response;
            if(sub.Type == "place")
            {
                response = await _subscriptionEndpoint.CreateGeographySubscription(sub.Latitude, sub.Longitude, sub.Radius);
            }
            else
            {
                response = await _subscriptionEndpoint.CreateTagSubscription(sub.Tag);
            }
            
            _log.Information("Subscription {0} created: {1} id: {2}", response.Data.Id, response.Data.Object, response.Data.Object_Id);
            await _subscriptions.CreateRealtimeSub(response, sub, User.Identity.Name);

            return Ok(response.Data);
        }

        public async Task<IHttpActionResult> Delete(string id)
        {
            if (id == "all")
            {
                await _subscriptionEndpoint.DeleteAllSubscriptions();
                _log.Information("all subscriptions deleted");
            }
            else
            {
                await _subscriptionEndpoint.DeleteSubscription(id);
                _subscriptions.DeleteSubscriptionAsync(User.Identity.Name, id);
                _log.Information("subscription {0} deleted", id);
            }

            return Ok();
        }

    }
}
