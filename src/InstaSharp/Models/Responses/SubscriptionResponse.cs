using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaSharp.Models.Responses {
    public class SubscriptionResponse : IResponse {

        public Meta Meta { get; set; }
        public Subscription Data { get; set; }
    }
}
