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

namespace CountingKs.Controllers
{
    public class DiaryEntriesController : BaseApiController
    {
        private ICountingKsIdentityService _identityService;

        public DiaryEntriesController(ICountingKsRepository repo, ICountingKsIdentityService identityService)
            : base(repo)
        {
            _identityService = identityService;
        }

        //GET: Return All Diary Entries
        public IEnumerable<DiaryEntryModel> Get(DateTime diaryId)
        {

            var results = TheRepository.GetDiaryEntries(_identityService.CurrentUser, diaryId)
                                       .ToList()
                                       .Select(e => TheModelFactory.Create(e));
            return results;
        }

        //GET: Return Specific Diary Entry
        public HttpResponseMessage Get(DateTime diaryId, int id)
        {
            var results = TheRepository.GetDiaryEntry(_identityService.CurrentUser, diaryId, id);

            if (results == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(results));
        }

        //POST: Create Diary Entries
        public HttpResponseMessage Post(DateTime dairyId, [FromBody]DiaryEntryModel model)
        {
            try
            {
                //Get entity that user passes in
                var entity = TheModelFactory.Parse(model);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read Diary Entry from body");
                }


                //Get Diary Entry that User is requesting
                var diary = TheRepository.GetDiary(_identityService.CurrentUser, dairyId);
                if (diary == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                //Check Duplicates
                if (diary.Entries.Any(e => e.Measure.Id == entity.Measure.Id))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Duplicate measure not allowed");
                }

                //Save Diary entry
                diary.Entries.Add(entity);
                if (TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not save to database ");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Delete
        public HttpResponseMessage delete(DateTime diaryId, int id)
        {
            try
            {
                if (TheRepository.GetDiaryEntries(_identityService.CurrentUser, diaryId).Any(e => e.Id == id) == false)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (TheRepository.DeleteDiaryEntry(id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpPatch]
        [HttpPut]
        //Put 
        public HttpResponseMessage Patch(DateTime diaryid, int id, [FromBody]DiaryEntryModel content)
        {
            try
            {
                //Get Diary entry from Repo
                var entity = TheRepository.GetDiaryEntry(_identityService.CurrentUser, diaryid, id);
                if(entity==null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                //Get Diary Content to be patched from User
                var parsedValue = TheModelFactory.Parse(content);
                if (parsedValue == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read Diary Entry from body");
                }

                if (entity.Quantity != parsedValue.Quantity)
                {
                    entity.Quantity = parsedValue.Quantity;
                    if(TheRepository.SaveAll())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }

            catch (Exception ex)
            {
                
                return Request.CreateResponse(HttpStatusCode.BadRequest,ex);
            }
        }

    }
}
