using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Microsoft.PWABuilder.ManifestCreator
{
    public static class Function
    {
        [Function("Create")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function");
            var queryString = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

            // Grab the URL. Prepend https:// to it if necessary.
            var url = queryString["url"];
            if (url != null && !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = $"https://{url}";
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                logger.LogError("Bad request. Expected query string to be absolute URL. Actual value {val}", url);
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                errorResponse.WriteString("URI must be validate, absolute URI");
                return errorResponse;
            }

            var manifestService = new ManifestService(uri, logger);
            var manifest = await manifestService.CreateManifest();
            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            await okResponse.WriteStringAsync(JsonSerializer.Serialize(manifest, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }));
            return okResponse;
        }
    }
}
