﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountingKs.Models
{
    public class FoodModel
    {
        public String URL { get; set; }
        public String Description{ get; set; }
        public IEnumerable<MeasureModel> Measure { get; set; }
    }
}