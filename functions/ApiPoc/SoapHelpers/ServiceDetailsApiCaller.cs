using System;
using System.Threading.Tasks;
using ServiceReference;

namespace ApiPoc.SoapHelpers
{
    public class EmploymentDetailsApiCaller : IEmploymentDetailsApiCaller
    {
        private readonly ApiCaller<SV_EMPLOYEES_PortTypeChannel> _apiCaller;

        public EmploymentDetailsApiCaller(ApiCaller<SV_EMPLOYEES_PortTypeChannel> apiCaller)
        {
            _apiCaller = apiCaller;
        }

        public Task<ServiceReference.EMPLOYMENTDETAILSRESPONSEV1> GetEmploymentDetailsAsync(string employmentNumber)
        {
            return _apiCaller.Call(x => x.REQUESTEMPLOYMENTDETAILSAsync(
                new EMPLOYMENTDETAILSREQUESTV1()

                {
                    HandlingAssertion = new HandlingAssertionType()
                    {
                        HandlingStatement = new HandlingStatementType()
                        {
                            DataClassification = new DataClassificationType()
                            {
                                ClassificationLabel = DataClassificationTypeClassificationLabel.PUBLIC
                            }
                        }
                    },
                    EmploymentDetailsRequestMessage = new EmploymentDetailsRequestMessage()
                    {
                        EmploymentDetailsRequest = new EmploymentDetailsRequestType()
                        {
                            Item = new searchKey1Type()
                            {
                                dateOfBirth = new DateTime(1980, 1, 1),
                                EmploymentNumber = employmentNumber
                            }
                        }
                    }
                }));
        }
    }
}