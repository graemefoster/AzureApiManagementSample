namespace ApiPoc.Weather
{
    public interface IWeatherRepository
    {
        Models.Weather? GetWeather(string postcode);
    }
}