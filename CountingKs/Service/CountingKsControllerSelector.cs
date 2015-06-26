using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CountingKs.Service
{
    public class CountingKsControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        public CountingKsControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        //Passes back description of controller : type, information
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //Gets all existing controllers in project
            var controllers = GetControllerMapping();

            //Get Route Data
            var routeData = request.GetRouteData();

            //From Webconfig.cs controllername will get it e.g. measure, food, etc
            var controllerName = (string)routeData.Values["controller"];

            HttpControllerDescriptor descriptor;

            //If New Controller found then return it
            if(controllers.TryGetValue(controllerName, out descriptor))
            {

                //Used to find V2 of our Controller if needed
                //var version = GetVersionFromQueryString(request);
                //var version = GetVersionFromHeader(request);
                //var version = GetVersionFromAcceptHeaderVersion(request);
                var version = GetVersionFromMediaType(request);

                var newName = string.Concat(controllerName, "V", version);
                HttpControllerDescriptor versionDescriptor;
                if(controllers.TryGetValue(newName, out versionDescriptor))
                {
                    return versionDescriptor;
                }
                return descriptor;
            }

            return null;

        }

        private String GetVersionFromMediaType(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;
            var ex = new Regex(@"application\/vnd\.countingks\.([a-z]+)\.v([0-9])+)\+json");
            foreach(var mime in acceptHeader)
            {
                var match = ex.Match(mime.MediaType);
                if(match != null)
                {
                    //returns version number
                    return match.Groups[2].Value;
                }
            }
            return "1";

        }

        private object GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;

            foreach(var mime in acceptHeader)
            {
                if(mime.MediaType == "application/json")
                {
                    var value = mime.Parameters
                                    .Where(v => v.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                                    .FirstOrDefault();
                    return value.Value;
                }
            }
            return "1";
        }

        private String GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "X-CountingKs-Version";
            if(request.Headers.Contains(HEADER_NAME))
            {
                var header = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (header!=null)
                {
                    return header;
                }
            }
            return "1" ;
        }


        //Used to query for a version
        private String GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];
            if(version!=null)
            {
                return null;
            }
            return "1";
        }
    }
}