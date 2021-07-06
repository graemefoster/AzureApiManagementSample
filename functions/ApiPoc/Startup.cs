using ApiPoc.SoapHelpers;
using ApiPoc.Weather;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceReference;

[assembly: FunctionsStartup(typeof(ApiPoc.Startup))]
namespace ApiPoc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ApiSettings>().Configure<IConfiguration>((settings, cfg) => cfg.GetSection("ApiPoc:Employee").Bind(settings));
            builder.Services.AddSingleton<IEmploymentDetailsApiCaller, EmploymentDetailsApiCaller>();
            builder.Services.AddSingleton<ApiCaller<SV_EMPLOYEES_PortTypeChannel>>();
            builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();
        }
    }
}