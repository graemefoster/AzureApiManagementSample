using System;
using System.Collections.Generic;
using System.Linq;
using ApiPoc.Models;

namespace ApiPoc.Weather
{
    internal class WeatherRepository : IWeatherRepository
    {
        private readonly IReadOnlyDictionary<string, Models.Weather> _weather;

        public WeatherRepository()
        {
            var weatherSeed = 25.4F;
            var rndSeed = new Random(340789234);
            _weather = Enumerable.Range(800, 9999)
                .Select(pc =>
                {
                    var rndFluctuation = (float) (2 * (rndSeed.NextDouble() - 1));
                    weatherSeed += rndFluctuation;
                    weatherSeed = Math.Max(30, Math.Min(15, weatherSeed));
                    return new Models.Weather()
                    {
                        Postcode = pc.ToString().PadLeft(4, '0'),
                        Temperatures = Enumerable.Range(0, 7).Select(i => new Temperature()
                        {
                            DaysAgo = i,
                            CelsiusMin = weatherSeed - (float) (rndSeed.NextDouble() * 15),
                            CelsiusMax = weatherSeed + (float) (rndSeed.NextDouble() * 15),
                        }).ToArray()
                    };
                }).ToDictionary(w => w.Postcode!, w => w);
        }

        public Models.Weather? GetWeather(string postcode)
        {
            if (_weather.TryGetValue(postcode, out var weather))
            {
                return weather;
            }

            return null;
        }
    }
}