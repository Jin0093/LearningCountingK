using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using CountingKs.Filters;
using WebApiContrib.Formatting.Jsonp;
using System.Web.Http.Dispatcher;
using CountingKs.Service;
using System.Net.Http.Headers;

namespace CountingKs
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Food",
                routeTemplate: "api/nutrition/foods/{foodid}",
                defaults: new { controller = "foods", foodid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Measures",
                routeTemplate: "api/nutrition/foods/{id}/measures/{id}",
                defaults: new { controller = "measures", id = RouteParameter.Optional }

            );

          //  config.Routes.MapHttpRoute(
          //    name: "Measures2",
          //    routeTemplate: "api/nutrition/foods/{id}/measures/{id}",
          //    defaults: new { controller = "measuresv2", id = RouteParameter.Optional }

          //);
            config.Routes.MapHttpRoute(
                name: "Diaries",
                routeTemplate: "api/user/diaries/{diaryid}",
                defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }

             );
            config.Routes.MapHttpRoute(
                name: "DiariesEntries",
                routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
                defaults: new { controller = "diaryentries", diaryid = RouteParameter.Optional }

            );
            config.Routes.MapHttpRoute(
                name: "DiarySummary",
                routeTemplate: "api/user/diaries/{diaryid}/summary",
                defaults: new { controller = "diarysummary" }

            );

            config.Routes.MapHttpRoute(
              name: "Token",
              routeTemplate: "api/token",
              defaults: new { controller = "Token" }

            );

            //Replace the Controller Configuration
            config.Services.Replace(typeof(IHttpControllerSelector),
                new CountingKsControllerSelector(config));



            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            //return Camelcase for Json formats:
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            CreateMediaTypes(jsonFormatter);

            //Adding support JSONP using : "webapicontrib.formatting.jsonp" in nugget
            var formatter = new JsonpMediaTypeFormatter(jsonFormatter, "cb");
            config.Formatters.Insert(0, formatter); // Makes API calls in jsonp formatter first

            //Adds a Https requirement for all API calls
            config.Filters.Add(new RequireHttpsAttribute());

            //Adds CORs By adding: "microsoft.aspnet.webapi.cors" in nugget
            config.EnableCors();
        }

        // Creates new media types for JSON formatter to be used for versioning
        private static void CreateMediaTypes(JsonMediaTypeFormatter jsonFormatter)
        {
            // Anything after vnd. ".." is a vendor
            var mediaTypes = new string[]
            {
                "application/vnd.countingks.food.v1+json",
                "application/vnd.countingks.measure.v1+json",
                "application/vnd.countingks.measure.v2+json",
                "application/vnd.countingks.diary.v1+json",
                "application/vnd.countingks.diaryEntry.v1+json"
            };

            foreach (var mediaType in mediaTypes)
            {
                jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
            }
        }
    }
}