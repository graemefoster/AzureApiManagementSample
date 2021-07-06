using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiPoc
{
    public static class HeaderFunction
    {
        [FunctionName("HeaderFunction")]
        public static async Task<Dictionary<string, Dictionary<string, string>>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest  req,
            ILogger logger)
        {
            logger.LogInformation("Calling simple header endpoint");

            var client = new HttpClient();
            var incomingFunctionHeaders = req.Headers.ToDictionary(x => x.Key, x => string.Join(',', x.Value));
            var incomingHeadersInAks =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    await client.GetStringAsync("http://api.poc.internal/weather/headers"));
            return new Dictionary<string, Dictionary<string, string>>()
            {
                {"atFunction", incomingFunctionHeaders},
                {"atAks", incomingHeadersInAks},
            };
        }
    }
}