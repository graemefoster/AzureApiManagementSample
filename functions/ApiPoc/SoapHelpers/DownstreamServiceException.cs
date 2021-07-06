using System;
using ServiceReference;

namespace ApiPoc.SoapHelpers
{
    public class DownstreamServiceException : Exception
    {
        public ResultsType DownstreamResponse { get; }

        public DownstreamServiceException(ResultsType downstreamResponse)
        {
            DownstreamResponse = downstreamResponse;
        }
    }
}