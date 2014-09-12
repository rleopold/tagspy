using InstaSharp.Extensions;
using InstaSharp.Models.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InstaSharp.Endpoints
{
    public class Subscription
    {
        public enum Object
        {
            User,
            Tag,
            Location,
            Geography
        }

        public enum Aspect
        {
            Media
        }

        private readonly InstagramConfig config;
        private readonly HttpClient client;

        public Subscription(InstagramConfig config)
        {
            this.config = config;
            client = new HttpClient {BaseAddress = new Uri(config.RealTimeApi)};
        }

        public Task<SubscriptionsResponse> List()
        {
            var request = new HttpRequestMessage { Method = HttpMethod.Get };
            request.RequestUri = new Uri(config.RealTimeApi);

            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientSecret);

            return client.ExecuteAsync<SubscriptionsResponse>(request);
        }

        public Task<SubscriptionResponse> Create(Object type, Aspect aspect)
        {
            // create a new guid that uniquely identifies this subscription request
            var verifyToken = Guid.NewGuid().ToString();
            var request = new HttpRequestMessage {Method = HttpMethod.Post};

            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientSecret);
            request.AddParameter("object", type.ToString().ToLower());
            request.AddParameter("aspect", aspect.ToString().ToLower());
            request.AddParameter("verify_token", verifyToken);
            request.AddParameter("callback_url", config.CallbackUri);

            return client.ExecuteAsync<SubscriptionResponse>(request);
        }

        public Task<SubscriptionResponse> CreateGeographySubscription(double latitude, double longitude, double radius)
        {
            var request = CreateSubscriptionBase();

            request.Content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string,string>("aspect", Aspect.Media.ToString().ToLower()),
                new KeyValuePair<string,string>("callback_url", config.CallbackUri),
                new KeyValuePair<string,string>("object", Object.Geography.ToString().ToLower()),
                new KeyValuePair<string,string>("lat", latitude.ToString()),
                new KeyValuePair<string,string>("lng", longitude.ToString()),
                new KeyValuePair<string,string>("radius", radius.ToString())

            });

            return client.ExecuteAsync<SubscriptionResponse>(request);
        }

        public Task<SubscriptionResponse> CreateTagSubscription(string tag)
        {
            var request = CreateSubscriptionBase();

            request.Content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("aspect", Aspect.Media.ToString().ToLower()),
                new KeyValuePair<string,string>("callback_url", config.CallbackUri),
                new KeyValuePair<string,string>("object", Object.Tag.ToString().ToLower()),
                new KeyValuePair<string,string>("object_id", tag)
            });

            return client.ExecuteAsync<SubscriptionResponse>(request);
        }

        public Task<SubscriptionResponse> DeleteSubscription(string id)
        {
            var request = new HttpRequestMessage { Method = HttpMethod.Delete };
            request.RequestUri = new Uri("https://api.instagram.com/v1/subscriptions/");

            request.AddParameter("id", id);
            if (id.ToCharArray().All(Char.IsDigit))
                request.AddParameter("object", "geography");
            else
                request.AddParameter("object", "tag");
            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientSecret);
            

            return client.ExecuteAsync<SubscriptionResponse>(request);

        }

        public Task<SubscriptionResponse> DeleteAllSubscriptions()
        {
            var request = new HttpRequestMessage { Method = HttpMethod.Delete };
            request.RequestUri = new Uri("https://api.instagram.com/v1/subscriptions/");

            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientSecret);
            request.AddParameter("object", "all");

            return client.ExecuteAsync<SubscriptionResponse>(request);

        }

        private HttpRequestMessage CreateSubscriptionBase()
        {
            // create a new guid that uniquely identifies this subscription request
            var verifyToken = Guid.NewGuid().ToString(); // might need to record this somewhere??
            var request = new HttpRequestMessage { Method = HttpMethod.Post };
            request.RequestUri = new Uri("https://api.instagram.com/v1/subscriptions/");

            request.AddParameter("verify_token", verifyToken);
            request.AddParameter("client_id", config.ClientId);
            request.AddParameter("client_secret", config.ClientSecret);

            return request;
        }

    }
}
