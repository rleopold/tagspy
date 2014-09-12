using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WindowsAzure.Table.Attributes;

namespace TagSpy.Models
{
    public class RealtimeSubscription
    {
        [PartitionKey]
        public string UserId { get; set; }
        [RowKey]
        public string Object_Id { get; set; }
        public string LastMediaIdRequest { get; set; }
        public string Type { get; set; }
        public string PlaceName { get; set; }
        public string Tag { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}