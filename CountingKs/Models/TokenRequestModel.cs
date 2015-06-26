using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountingKs.Models
{
    public class TokenRequestModel
    {
        public String APIKey { get; set; }
        public String Signature { get; set; }
    }
}