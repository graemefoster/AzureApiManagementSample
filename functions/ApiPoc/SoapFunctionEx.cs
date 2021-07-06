using System;
using System.Net;
using System.Threading.Tasks;
using ApiPoc.Models;
using ApiPoc.SoapHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiPoc
{
    /// <summary>
    /// For now, you cannot affect the result of a function in a (preview) function filter.
    /// https://github.com/Azure/azure-webjobs-sdk/issues/2546
    /// </summary>
    public static class SoapFunctionEx
    {
        public static async Task<IActionResult> Wrap<T>(this HttpRequest req, Func<Task<T>> func, ILogger logger)
        {
            try
            {
                return new OkObjectResult(await func());
            }
            catch (DownstreamServiceException dse)
            {
                var correlationId = Guid.NewGuid();
                logger.LogError(dse, "A downstream error was raised {Id}", correlationId);
                if (dse.DownstreamResponse.ProviderSystemError.ReturnType == "Validation")
                {
                    return new ObjectResult(new ErrorsResponse
                    {
                        Errors = new[]
                        {
                            new Error
                            {
                                Id = correlationId,
                                Detail = dse.DownstreamResponse.ReturnText,
                                Code = dse.DownstreamResponse.ReturnCode,
                                Source = new[]
                                {
                                    new SourceItem
                                    {
                                        Parameter = dse.DownstreamResponse.ProviderSystemError.ReturnCode
                                    }
                                }
                            }
                        }
                    }) {StatusCode = (int) HttpStatusCode.UnprocessableEntity};
                }

                if (dse.DownstreamResponse.ProviderSystemError.ReturnType == "Business")
                {
                    return new ObjectResult(new ErrorsResponse
                    {
                        Errors = new  Error[]
                        {
                            new Error
                            {
                                Id = correlationId,
                                Detail = dse.DownstreamResponse.ReturnText,
                                Code = dse.DownstreamResponse.ReturnCode,
                                Source = new[]
                                {
                                    new SourceItem
                                    {
                                        System = "employmentNumber"
                                    }
                                }
                            }
                        }
                    }) {StatusCode = (int) HttpStatusCode.BadRequest};
                }

                return new ObjectResult(new ErrorsResponse
                {
                    Errors = new[]
                    {
                        new Error
                        {
                            Id = correlationId,
                            Detail = dse.DownstreamResponse.ReturnText,
                            Code = dse.DownstreamResponse.ReturnCode
                        }
                    }
                }) {StatusCode = (int) HttpStatusCode.BadRequest};
            }
        }
    }
}