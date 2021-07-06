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

    public class EmployeeAddressFunction
    {
        private readonly IEmploymentDetailsApiCaller _apiCaller;

        public EmployeeAddressFunction(IEmploymentDetailsApiCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [FunctionName("EmployeeAddressDetails")]
        [OpenApiOperation(operationId: "EmployeeAddressDetails", tags: new[] { "Employee" })] 
        [OpenApiParameter(name: "employmentNumber", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "employmentNumber Identifier")] 
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ErrorsResponse), Description = "Details of the errors that occurred")] 
        [OpenApiResponseWithBody(HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(EmployeeSummary), Description = "The OK response")] 
        public Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Employee/{employmentNumber:regex(^[a-zA-Z0-9]{{8}}$)}/address")]
            HttpRequest req,
            ILogger logger,
            string employmentNumber)
        {
            return req.Wrap(async () =>
            {
                logger.LogInformation(
                    "Calling SOAP endpoint to retrieve home and postal address information for {employmentNumber}",
                    employmentNumber);

                var personalSummaryData = (await _apiCaller.GetEmploymentDetailsAsync(employmentNumber))
                    .EmploymentDetailsResponseMessage.EmploymentDetailsResponse.personalSummaryData;
                
                return new EmployeeSummary()
                {
                    EmploymentNumber = personalSummaryData.EmploymentNumber,
                    PostalAddress = personalSummaryData.postalAddress,
                    ResidentialAddress = personalSummaryData.residentialAddress,
                    Phone = personalSummaryData.phone,
                    EmailAddress = personalSummaryData.emailAddress
                };
            }, logger);
        }
    }
}