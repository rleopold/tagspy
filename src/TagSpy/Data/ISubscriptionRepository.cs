using System;
using System.Threading.Tasks;
namespace TagSpy.Data
{
    public interface ISubscriptionRepository
    {
        Task CreateRealtimeSub(InstaSharp.Models.Responses.SubscriptionResponse response, TagSpy.Models.CreateSub create, string user);
        Task<System.Collections.Generic.List<TagSpy.Models.RealtimeSubscription>> GetPlaceSubscriptionsByUserAsync(string user);
        Task<System.Collections.Generic.List<TagSpy.Models.RealtimeSubscription>> GetSubscriptionListByUserAsync(string user);
        Task<System.Collections.Generic.List<TagSpy.Models.RealtimeSubscription>> GetTagSubscriptionsByUserAsync(string user);
        bool HasMinMediaBeenServed(string user, string object_id, string minMediaId);
        Task DeleteSubscriptionAsync(string user, string object_id);
        void MarkMinMediaServed(string user, string object_id, string minMediaId);
    }
}
