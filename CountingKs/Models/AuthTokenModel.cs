using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountingKs.Models
{
    public class AuthTokenModel
    {
        public String Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}