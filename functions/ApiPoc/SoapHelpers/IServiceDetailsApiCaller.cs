using System.Threading.Tasks;

namespace ApiPoc.SoapHelpers
{
    public interface IEmploymentDetailsApiCaller
    {
        Task<ServiceReference.EMPLOYMENTDETAILSRESPONSEV1> GetEmploymentDetailsAsync(string employmentNumber);
    }
}