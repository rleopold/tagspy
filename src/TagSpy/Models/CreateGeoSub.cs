using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagSpy.Models
{
    public class CreateSub
    {
        public string Type { get; set; }      //either "tag" or "place"
        public string Tag { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }

    }
}