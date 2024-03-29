﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VGT.Common.Models
{
    public class ToornamentToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }

    public class InternalToornamentToken
    {
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
