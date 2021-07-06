using System.Net;
using ApiPoc.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ApiPoc
{
    public class WeatherLookup
    {
        private readonly IWeatherRepository _weatherRepository;

        public WeatherLookup(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        [OpenApiOperation(operationId: "WeatherLookup", tags: new[] {"weather"})]
        [OpenApiParameter(name: "postcode", In = ParameterLocation.Path, Required = true, Type = typeof(string),
            Description = "Postcode")]
        [OpenApiResponseWithoutBody(HttpStatusCode.NotFound, Description = "No such postcode")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json",
            typeof(Models.Weather),
            Example = typeof(WeatherExample),
            Description = "Last 7 days of weather")]
        [FunctionName("Weather")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "employee/weather/{postcode}")]
            HttpRequest req,
            ILogger logger,
            string postcode)
        {
            var result = _weatherRepository.GetWeather(postcode);
            if (result == null) return new NotFoundResult();
            return new OkObjectResult(result);
        }
    }
}