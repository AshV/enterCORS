using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace enterCORS
{
    public class Invoke
    {
        private readonly ILogger<Invoke> _logger;

        public Invoke(ILogger<Invoke> log)
        {
            _logger = log;
        }

        [FunctionName("Invoke")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "url" })]
        [OpenApiParameter(name: "url", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "URL")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "api/{url?}")] HttpRequest req, string url)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

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

            _logger.LogInformation("Execution Completed.");
            return new OkObjectResult("Unsupported Request.");
        }
    }
}