using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using CountingKs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Filters;

namespace CountingKs.Controllers
{
    //[Authorize] : Piggy-backing on ASP.net Authentication, only authorized users allowed to call
    [CountingKsAuthorized] //Using Custom Basic Authentication located in filters folder "CountingKsAuthorizedAttribute"

    public class DiariesController : BaseApiController
    {
        private ICountingKsIdentityService _identityService;

        public DiariesController(ICountingKsRepository repo, ICountingKsIdentityService identityService) : base(repo)
        {
            _identityService = identityService;
        }

        //GET: Return All Diary Entries
        public IEnumerable<DiaryModel> Get()
        {
            var username = _identityService.CurrentUser;
            var results = TheRepository.GetDiaries(username)
                                       .OrderByDescending(d => d.CurrentDate)
                                       .Take(10)
                                       .ToList()
                                       .Select(d => TheModelFactory.Create(d));
            return results;
        }

        //GET: Return Specific Diary Entry
        public HttpResponseMessage Get(DateTime diaryId)
        {
            var username = _identityService.CurrentUser;
            var results = TheRepository.GetDiary(username, diaryId);

            if (results == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else

                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(results));
        }

        //POST: Update Diary Entries
        public object Post(DateTime dairyId, [FromBody]DiaryEntryModel model)
        {
            try
            {
                //Get entity that user passes in
                var entity = TheModelFactory.Parse(model);
                if(entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read Diary Entry from body");
                }


                //Get Diary Entry that User is requesting
                var diary = TheRepository.GetDiary(_identityService.CurrentUser, dairyId);
                if (diary == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //Save Diary entry
                diary.Entries.Add(entity);
                TheRepository.SaveAll();

                return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
