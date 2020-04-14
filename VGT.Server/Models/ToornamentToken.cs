using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VGT.Server.Models
{
    public class ToornamentToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }

    public class InternalToornamentToken
    {
        public string scope { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
}
