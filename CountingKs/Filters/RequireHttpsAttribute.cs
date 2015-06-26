using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Text;

namespace CountingKs.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)//actioncontext has access to request and response object
        {
            var req = actionContext.Request;

            //Check to see of request is an Https request
            if (req.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var html = "<p> Https is required </p>";
                //If its a GET request tell user if its there or not.
                if (req.Method.Method == "GET")
                {

                    //Changes to URI to https from http
                    actionContext.Response = req.CreateResponse(HttpStatusCode.Found);
                    actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");
                    var uriBuilder = new UriBuilder(req.RequestUri);
                    uriBuilder.Scheme = Uri.UriSchemeHttps;
                    uriBuilder.Port = 443; //ss port
                    actionContext.Response.Headers.Location = uriBuilder.Uri;
                }
                else
                {
                    actionContext.Response = req.CreateResponse(HttpStatusCode.NotFound);
                    actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");
                }

            }
        }
    }
}
