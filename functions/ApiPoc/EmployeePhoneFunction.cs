using System.Net;
using System.Threading.Tasks;
using ApiPoc.Models;
using ApiPoc.SoapHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ServiceReference;

namespace ApiPoc
{
    public class EmployeePhoneFunction
    {
        private readonly IEmploymentDetailsApiCaller _apiCaller;

        public EmployeePhoneFunction(IEmploymentDetailsApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [OpenApiOperation(operationId: "EmployeePhoneDetails", tags: new[] {"Employee"})]
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "employmentNumber Identifier")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(phoneType[]), Description = "Phone details")]
        [FunctionName("EmployeePhoneDetails")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "Employee/{employmentNumber}/phone")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber)
        {
            return req.Wrap(async () =>
            {
                logger.LogInformation("Calling SOAP endpoint to retrieve phone numbers for {employmentNumber}",
                    employmentNumber);

                return (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber))
                    .EmploymentDetailsResponseMessage.EmploymentDetailsResponse.personalSummaryData.phone;
                
            }, logger);
        }
    }
}