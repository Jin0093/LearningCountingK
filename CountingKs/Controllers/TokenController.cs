using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Models;
using System.Text;

namespace CountingKs.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(ICountingKsRepository repo)
            : base(repo)
        {

        }

        public HttpResponseMessage Post([FromBody]TokenRequestModel model)
        {
            try
            {
                var user = TheRepository.GetApiUsers().Where(u => u.AppId == model.APIKey).FirstOrDefault();
                if(user!=null)
                {
                    var secret = user.Secret;

                    /*
                    // Simple implementation 
                    var key = Convert.FromBase64String(secret);
                    var provider = new System.Security.Cryptography.HMACSHA256(key);
                    // Compute Has from API Key
                    var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(apiUser.AppId));
                    var signature = Convert.ToBase64String(hash);

                    if(signature == model.Signature)
                    {
                        var rawTokenInfo = string.Concat(apiUser.Appid + DateTime.UtcNow.ToString("d"));
                        var rawTokenByte = Encoding.UTF8.getBytes(rawTokeninfo)
                        var token = provider.ComputeHash(rawTokenByte);
                        var authToken = new AuthToken()
                       {
                     * Token = Convert.ToBase64Stringtoken(token),
                       Expiration = Datetime.UtcNow.AddDays(7)
                       ApiUser = user
                       };
                     * if (Threpository.Insert(authToken) && TheRepository.SaveAll()
                     * {
                     * return Request.Createresponse(HttpStatusCode.Created, TheModelFactory.Create(authToken));
                     * }
                     
                    }
                     * */
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
