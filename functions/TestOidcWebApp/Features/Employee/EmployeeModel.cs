using TestOidcWebApp.EmployeeApi;

namespace TestOidcWebApp.Features.Employee
{
    public class EmployeeModel
    {
        public string EmploymentNumber { get; set; }
        public EmployeeSummary Summary { get; set; }
    }
}