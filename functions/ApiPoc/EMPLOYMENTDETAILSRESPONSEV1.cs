// ReSharper disable once CheckNamespace
namespace ServiceReference
{
    public partial class EMPLOYMENTDETAILSRESPONSEV1: ISoapHelper
    {
        public ResultsType GetResult()
        {
            return EmploymentDetailsResponseMessage?.EmploymentDetailsResponseResults ?? new ResultsType { ReturnType = ResultsTypeReturnType.SUCCESS};
        }
    }
}