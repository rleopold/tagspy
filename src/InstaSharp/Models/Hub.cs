using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaSharp.Models
{
    /// <summary>
    /// Use this class for the challenge response when creatig a subscription
    /// </summary>
    public class hub
    {
        public string mode { get; set; }
        public string challenge { get; set; }
        public string verify_token { get; set; }
    }
}
