using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using work.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
// using Google.Apis.Auth.OAuth2;
// //using Google.Cloud.Datastore.V1;
// using Google.Cloud.BigQuery.V2;

namespace work.Controllers
{
    public class HomeController : Controller
    {
        private ILogger logger;
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;

        }

        public IActionResult Index()
        {
            logger.LogInformation("====INDEX CALLED=====");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Search(bool contain, string selected, string original, string keyword)
        {
            //var url = "http://localhost:9200/sample/product/_search";
            var url = "http://35.200.38.9:9200/sample/product/_search";

            //var q = string.Format(@"{ ""query"" : { ""match"" : { ""name"" : ""{0}"" } } }", keyword);
            var q = CreateQuery(keyword).ToString();

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(q, Encoding.UTF8, "application/json");

            dynamic buffer = null;
            using (var client = new HttpClient())
            {
                var res = await client.SendAsync(req);
                if(!res.IsSuccessStatusCode)
                {
                    return this.NotFound();
                }
                var str = await res.Content.ReadAsStringAsync();
                buffer = JObject.Parse(str);
            }

            JArray hits = buffer.hits.hits;
            var env = new SearchResultEnvelope(false)
            {
                results = hits.Select(hit => hit["_source"]).Select(jt => jt.FromSearchHit()).ToArray()
            };

            return this.Ok(env);
        }

        private static JObject CreateQuery(string keyword)
        {
            return new JObject
            {
                ["query"] = new JObject
                {
                    ["multi_match"] = new JObject
                    {
                        ["query"] = keyword,
                        ["fields"] = new JArray { "name","description" }
                    }
                } 
            };
            //return JObject.FromObject
            //    (
            //        new
            //        {
            //            query = new JObject {
            //            }
            //            {
            //                match = new JObject { ["name"] = keyword }
            //            }
            //        }
            //    );
        }
    }
}
