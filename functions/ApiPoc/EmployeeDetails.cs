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

namespace ApiPoc
{
    public class EmployeeDetails
    {
        private readonly IEmploymentDetailsApiCaller _apiCaller;

        public EmployeeDetails(IEmploymentDetailsApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [OpenApiOperation(operationId: "EmployeeDetails", tags: new[] {"Employee"})]
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "employmentNumber Identifier")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(EmployeeSummary), Description = "The OK response", Example = typeof(EmployeeSummaryExample))]
        [FunctionName("EmployeeDetails")]
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",
                Route = "Employee/{employmentNumber:regex(^[a-zA-Z0-9]{{8}}$)}")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber)
        {
            logger.LogInformation("Calling SOAP endpoint to retrieve employee information for {employmentNumber}",
                employmentNumber);

            return req.Wrap(
                async () =>
                {
                    var details = (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber)).EmploymentDetailsResponseMessage
                        .EmploymentDetailsResponse.personalSummaryData;
                    return new EmployeeSummary()
                    {
                        Phone = details.phone,
                        EmailAddress = details.emailAddress,
                        PostalAddress = details.postalAddress,
                        ResidentialAddress = details.residentialAddress,
                        EmploymentNumber = details.EmploymentNumber
                    };
                },
                logger);
        }
    }
}