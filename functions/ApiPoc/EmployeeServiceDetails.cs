using System.Linq;
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
    public class EmployeeEmploymentDetails
    {
        private readonly IEmploymentDetailsApiCaller _apiCaller;

        public EmployeeEmploymentDetails(IEmploymentDetailsApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [OpenApiOperation(operationId: "EmployeeEmploymentDetails", tags: new[] {"Employee"})]
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string),
            Description = "employmentNumber Identifier")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json",
            bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json",
            bodyType: typeof(employmentDetailsType[]), Description = "Employment details")]
        [FunctionName("EmployeeEmploymentDetails")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "Employee/{employmentNumber}/employment-details")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber)
        {
            logger.LogInformation("Calling SOAP endpoint to retrieve employee information for {EmploymentNumber}", employmentNumber);

            return req.Wrap(
                async () => (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber)).EmploymentDetailsResponseMessage
                    .EmploymentDetailsResponse.employmentDetails,
                logger);
        }
    }
}