using InstaSharp.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TagSpy.Models;
using WindowsAzure.Table;

namespace TagSpy.Data
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private ITableSet<RealtimeSubscription> _tableSet;

        public SubscriptionRepository(ITableSet<RealtimeSubscription> tableSet)
        {
            _tableSet = tableSet;
        }

        public Task<List<RealtimeSubscription>> GetSubscriptionListByUserAsync(string user)
        {
            return Task.Run(() => 
                 _tableSet.Where(s => s.UserId == user).ToList()
            );
        }

        public Task<List<RealtimeSubscription>>  GetPlaceSubscriptionsByUserAsync(string user)
        {
            return Task.Run(() =>
                 _tableSet.Where(s => s.UserId == user && s.Type == "place").ToList()
            );
        }

        public Task<List<RealtimeSubscription>> GetTagSubscriptionsByUserAsync(string user)
        {
            return Task.Run(() =>
                 _tableSet.Where(s => s.UserId == user && s.Type == "tag").ToList()
            );
        }

        public bool HasMinMediaBeenServed(string user, string object_id, string minMediaId)
        {
            var rt = _tableSet.FirstOrDefault(r => r.Object_Id == object_id && r.UserId == user);
            if (rt != null && rt.LastMediaIdRequest != minMediaId)
            {
                return false;
            }

            return true; //in this case sink the request
        }

        public void MarkMinMediaServed(string user, string object_id, string minMediaId)
        {
            var rt = _tableSet.FirstOrDefault(r => r.Object_Id == object_id && r.UserId == user);
            if (rt != null)
                rt.LastMediaIdRequest = minMediaId;
            _tableSet.UpdateAsync(rt);
        }

        public async Task DeleteSubscriptionAsync(string user, string object_id)
        {
            var rt = _tableSet.FirstOrDefault(r => r.Object_Id == object_id && r.UserId == user);
            if (rt != null)
                await _tableSet.RemoveAsync(rt);
        }

        public async Task CreateRealtimeSub(SubscriptionResponse response, CreateSub create, string user)
        {
            var rt = new RealtimeSubscription();
            rt.Type = create.Type;
            rt.UserId = user;
            rt.Object_Id = response.Data.Object_Id;
            rt.LastMediaIdRequest = "0";
            rt.Tag = create.Tag;
            rt.Latitude = create.Latitude;
            rt.Longitude = create.Longitude;
            await _tableSet.AddOrUpdateAsync(rt);
        }

    }
}