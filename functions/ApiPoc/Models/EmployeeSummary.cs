using ServiceReference;

namespace ApiPoc.Models
{
    public class EmployeeSummary
    {
        public string? EmploymentNumber { get; set; }
        public addressType? ResidentialAddress { get; set; }
        public addressType? PostalAddress { get; set; }
        public phoneType[]? Phone { get; set; }
        public string? EmailAddress { get; set; }
    }
}