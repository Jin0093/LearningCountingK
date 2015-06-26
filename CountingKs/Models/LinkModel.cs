using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountingKs.Models
{
    public class LinkModel
    {
        public String Href { get; set; }
        public String Rel { get; set; }
        public String Method { get; set; }
        public bool IsTemplated { get; set; } //Does link has template references
    }
}