using System;

namespace ApiPoc.Models
{
    public class Weather
    {
        public string? Postcode { get; set; }
        public Temperature[]? Temperatures { get; set; }
    }
}