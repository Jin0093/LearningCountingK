using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Text;
using WebMatrix.WebData;
using System.Security.Principal;
using System.Threading;




namespace CountingKs.Filters
{
    public class CountingKsAuthorizedAttribute : AuthorizationFilterAttribute // This is Filter 
    {
        //If Unauthorized method is called and = emty/null then continue down pipeline but attaches unauthorized acccess message
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //If user is already Autehnticated, shortcut it
            if(Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }
            //Create varrible to check if header within request has authorization
            var authHeader = actionContext.Request.Headers.Authorization;

            if(authHeader!=null)
            {
                //Checks header variable scheme == basic and Users credentials != whites space/null
                //If valid, then you have authorized user with authorized Credentials
                if(authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    //Get Username & Password from header
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var split = credentials.Split(':');
                    var username = split[0];
                    var password = split[1];

                    if(!WebSecurity.Initialized)
                    {
                        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    }

                    if(WebSecurity.Login(username,password))//Replace code here for own username/password authentacation
                    {
                        //Set identity to new service
                        var principal = new GenericPrincipal(new GenericIdentity(username), null);
                        Thread.CurrentPrincipal = principal;
                        return;
                    }

                }
            }

            //Handles all unauthorized requests
            HandleUnauthorized(actionContext);
        }

        private void HandleUnauthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Create new Response
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //Compute URI for CountingKs/account/login
            actionContext.Response.Headers.Add("WWWW-Authenticate", 
                "Basic Scheme = 'CountingKs' location = /account/login"); //Tell User why they can't be authenticate



            
        }
    }
}