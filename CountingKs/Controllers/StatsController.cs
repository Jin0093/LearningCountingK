using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using System.Web.Http.Cors;

namespace CountingKs.Controllers
{
   // [EnableCors("*","X-OURAPP","GET")]//origin that is supported, which header is supported, which methods
    [RoutePrefix("api/stats")]
    public class StatsController : BaseApiController
    {
        public StatsController(ICountingKsRepository repo) : base(repo)
        {

        }
        [Route ("")]//Routing by Attributes
        public IHttpActionResult Get()
        {
            var results = new
            {
                NumFoods = TheRepository.GetAllFoods().Count(),
                NumUsers = TheRepository.GetApiUsers().Count()
            };
            return Ok(results);
        }

        [Route("~/api/stat/{id:int}")]//Routing by Attributes parameters, 
        public IHttpActionResult Get(int id)
        {
            if(id==1)
            {
                return Ok(new { NumUsers = TheRepository.GetAllFoods().Count()});
            }

            if(id==2)
            {
                return Ok(new { NumUsers = TheRepository.GetApiUsers().Count() });
            }

            return NotFound();
        }
        [DisableCors()]
        [Route("~/api/stat/{name:alpha}")]
        public IHttpActionResult Get(string name)
        {
            if (name == "food")
            {
                return Ok(new { NumUsers = TheRepository.GetAllFoods().Count() });
            }

            if (name == "user")
            {
                return OK(new { NumUsers = TheRepository.GetApiUsers().Count() });
            }

            return NotFound();
        }
    }
}
