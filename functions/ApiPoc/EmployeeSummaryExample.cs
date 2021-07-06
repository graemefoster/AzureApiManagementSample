using ApiPoc.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ServiceReference;

namespace ApiPoc
{
    public class EmployeeSummaryExample : OpenApiExample<EmployeeSummary>
    {
        public EmployeeSummary Fred = new EmployeeSummary()
        {
            EmailAddress = "fred.fibnar@fred.com",
            EmploymentNumber = "12345678",
            Phone = new []{ new phoneType()
            {
                number = "01234567891",
                type = phoneTypeType.HOME
            }},
            PostalAddress = new addressType()
            {
                addressLine =new [] { "100 Acacia Avenue"},
                city = "Perth",
                country = "Australia",
                state = "WA",
                postCode = "6000"
            },
            ResidentialAddress = new addressType()
            {
                addressLine =new [] { "100 Acacia Avenue"},
                city = "Perth",
                country = "Australia",
                state = "WA",
                postCode = "6000"
            }
        };

        public override IOpenApiExample<EmployeeSummary> Build(NamingStrategy? namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Sample Person", Fred, namingStrategy));
            return this;
        }
    }
}