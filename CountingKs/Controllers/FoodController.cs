using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Models;
using CountingKs.Data.Entities;

namespace CountingKs.Controllers
{
    public class FoodController : ApiController
    {
        //Waant interface as a parameter so we are decoupled with the Repo and be able to swap repo's
        //Don'tcare what repo is

        ICountingKsRepository _repo;
        ModelFactory _modelFactory;
        public FoodController(ICountingKsRepository repo)
        {
            _repo = repo;
            _modelFactory = new ModelFactory();
        }
        public IEnumerable<FoodModel> Get(bool includeMeasures = true)
        {
            IQueryable<Food> query;
            if(includeMeasures)
            {
                query = _repo.GetAllFoodsWithMeasures()
            }
            else{
                 query = _repo.GetAllFoods()
            }
           var results = query.OrderBy(f => f.Description).Take(25).ToList().Select(f => _modelFactory.Create(f));

            return results;
        }

        //Return individual of Foods
        public FoodModel Get(int id)
        {
            return _modelFactory.Create(_repo.GetFood(id));
        }
    }
}
