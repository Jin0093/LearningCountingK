using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using CountingKs.Data;
using CountingKs.Models;
using CountingKs.Data.Entities;
using CountingKs.Filters;

namespace CountingKs.Controllers
{
    [RequireHttps]
    public class FoodController : BaseApiController
    {
        //Waant interface as a parameter so we are decoupled with the Repo and be able to swap repo's
        //Don'tcare what repo is

        const int PAGE_SIZE = 50;

        public FoodController(ICountingKsRepository repo) : base(repo)
        {

        }
        public object Get(bool includeMeasures = true, int page = 0)
        {
            IQueryable<Food> query;
            if(includeMeasures)
            {
                query = TheRepository.GetAllFoodsWithMeasures();
            }
            else{
                query = TheRepository.GetAllFoods();
            }
            var baseQuery = query.OrderBy(f => f.Description);

            // Paging and Displaying useful Information to user:
            // Begin
            var totalCount = baseQuery.Count(); // Number of items
            var totalPages = Math.Ceiling((double)totalCount / PAGE_SIZE); // Total Pages

            var helper = new UrlHelper(Request);
            var prevUrl = page > 0 ? helper.Link("Food", new {page = page - 1}) : "";
            var NextUrl = page < totalPages-1 ? helper.Link("Food", new {page = page + 1}) : "";
    
            var results = baseQuery.Skip(PAGE_SIZE * page) // Paging
                                   .Take(PAGE_SIZE)
                                   .ToList()
                                   .Select(f => TheModelFactory.Create(f));
            // End

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageUrl = prevUrl,
                NextPageUrl = NextUrl,
                Results = results
            };
        }

        //Return individual of Foods
        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheRepository.GetFood(foodid));
        }
    }
}
