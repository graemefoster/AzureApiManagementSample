using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace TestOidcWebApp.EmployeeApi
{
    public class EmployeeApis
    {
        private readonly HttpClient _client;
        private readonly IOptions<AppSettings> _settings;

        public EmployeeApis(HttpClient client, ILogger<EmployeeApis> logger, IOptions<AppSettings> settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task<EmployeeSummary> GetSummary(string employmentNumber)
        {
            return await CallApiAsync<EmployeeSummary>($"Employee/v1/{employmentNumber}");
        }

        [ItemCanBeNull]
        public async Task<TResultType> CallApiAsync<TResultType>(string requestUri)
        {
            return JsonConvert.DeserializeObject<TResultType>(await CallApiRawAsync(requestUri));
        }

        public async Task<string> CallApiRawAsync(string requestUri)
        {
            if (!_settings.Value.AddVersionToUrl)
            {
                requestUri = requestUri.Replace("/v1/", "/"); //to make local debugging easier
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return responseJson;
        }
    }
}