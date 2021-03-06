﻿using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CountingKs.Controllers
{
    public class MeasuresV2Controller : BaseApiController
    {
        public MeasuresV2Controller(ICountingKsRepository repo)
            : base(repo)
        {
        }

        //Get All Measurements for specific food
        public IEnumerable<MeasureV2eModel> Get(int foodid)
        {
            var results = TheRepository.GetMeasuresForFood(foodid)
                               .ToList()
                               .Select(m => TheModelFactory.Create2(m));
            return results;
        }

        //Get Specific Measurement
        public MeasureV2eModel Get(int foodid, int id)
        {
            var results = TheRepository.GetMeasure(id);
            if (results.Food.Id == foodid)
            {
                return TheModelFactory.Create2(results);
            }
            return null;

        }
    }
}
