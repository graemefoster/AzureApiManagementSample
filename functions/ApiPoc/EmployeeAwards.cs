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
    public class EmployeeAwards
    {
        private readonly IEmploymentDetailsApiCaller _apiCaller;

        public EmployeeAwards(IEmploymentDetailsApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [OpenApiOperation(operationId: "EmploymentAndAwards", tags: new[] {"employee"})]
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Employment Number")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(employmentAwardsType[]), Description = "The OK response")]
        [FunctionName("EmployeeAwards")]
        public Task<IActionResult> RunEmployeeAwards([HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "employee/{employmentNumber}/awards")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber)
        {
            logger.LogInformation("Calling SOAP endpoint to retrieve employee information for {employmentNumber}",
                employmentNumber);
            
            return req.Wrap(
                async () =>
                {
                    var employmentDetailsAsync = (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber));
                    return employmentDetailsAsync.EmploymentDetailsResponseMessage
                        .EmploymentDetailsResponse.employmentAwards;
                },
                logger);
            
        }

        [OpenApiOperation(operationId: "RunEmployeeAward", tags: new[] {"Employee"})]
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "employmentNumber Identifier")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Award identifier")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(employmentAwardsType), Description = "The OK response")]
        [FunctionName("EmployeeAward")]
        public Task<IActionResult> RunEmployeeAward(
            [HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "Employee/{employmentNumber}/awards/{id}")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber,
            string id)
        {
            logger.LogInformation("Calling SOAP endpoint to retrieve employee information for {employmentNumber}",
                employmentNumber);
            
            return req.Wrap(
                async () =>  (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber)).EmploymentDetailsResponseMessage.EmploymentDetailsResponse.employmentAwards.SingleOrDefault(x => x.code == id),
                logger);
            
        }

    }
}