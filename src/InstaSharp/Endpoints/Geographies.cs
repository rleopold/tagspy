﻿using InstaSharp.Extensions;
using InstaSharp.Models.Responses;
using System.Threading.Tasks;

namespace InstaSharp.Endpoints {
    public class Geographies : InstagramApi {

        /// <summary>
        /// Geographies Endpoints
        /// </summary>
        /// <param name="config">An instance of the InstagramConfig class.</param>
        /// <param name="auth">An instance of the OAuthResponse class.</param>
        public Geographies(InstagramConfig config, OAuthResponse auth = null)
            : base("geographies/", config, auth) { }

        /// <summary>
        /// Get very recent media from a geography subscription that you created. Note: you can only access Geographies that were explicitly created by your OAuth client. To backfill photos from the location covered by this geography, use the media search endpoint.
        /// <para>
        /// <c>Requires Authentication: </c>False
        /// </para>
        /// </summary>
        /// <param name="mediaId">The id of the media about which to retrieve data</param>
        /// <param name="count">Max number of media to return.</param>
        /// <param name="min_id">Return media before this min_id.</param>
        public Task<MediasResponse> Recent(int geoId, int? count = null, string min_id = "") {
            var request = base.Request(string.Format("{0}/media/recent", geoId));
            request.AddParameter("count", count);
            request.AddParameter("min_id", min_id);
            return base.Client.ExecuteAsync<MediasResponse>(request);
        }
    }
}
