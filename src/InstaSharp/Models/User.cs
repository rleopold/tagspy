﻿using System;

namespace InstaSharp.Models {
    public class User : UserInfo
    {
        public string Bio { get; set; }
        public string Website { get; set; }
        public Count Counts { get; set; }
    }
}
