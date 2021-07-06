using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace ApiPoc.Weather
{
    public class WeatherExample : OpenApiExample<Models.Weather>
    {
        public override IOpenApiExample<Models.Weather> Build(NamingStrategy? namingStrategy = null)
        {
            var weather = new WeatherRepository().GetWeather("6000");
            Examples.Add(OpenApiExampleResolver.Resolve("Sample Weather", weather, namingStrategy));
            return this;
        }
    }
}