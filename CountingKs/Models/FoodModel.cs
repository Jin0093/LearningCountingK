using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountingKs.Models
{
    public class FoodModel
    {
        public String Description{ get; set; }
        public IEnumerable<MeasureModel> Measure { get; set; }
    }
}