using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace enterCORS
{
    public static class Invoke
    {
        [FunctionName("Invoke")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "api/{url?}")] HttpRequest req,
            string url,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method.ToLowerInvariant().Equals("get"))
            {
                if (string.IsNullOrEmpty(url)) return new OkObjectResult("GET request is expecting a URL. Please refer documentation.");

                if (!url.ToLowerInvariant().StartsWith("http")) url = "http://" + url;

                Uri finalUri;
                if (!Uri.TryCreate(url, UriKind.Absolute, out finalUri)) return new OkObjectResult($"URL {finalUri} doesn't seem proper, please verify.");

                var http = new HttpClient();
                return new OkObjectResult(await http.GetStringAsync(finalUri));
            }

            if (req.Method.ToLowerInvariant().Equals("post"))
            {
            }

            log.LogInformation("Execution Completed.");
            return new OkObjectResult("Unsupported Request.");
        }
    }
}
