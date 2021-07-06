namespace TestOidcWebApp.EmployeeApi
{
    public class EmployeeSummary
    {
        public string EmploymentNumber { get; set; }
        public Address ResidentialAddress { get; set; }
        public Address PostalAddress { get; set; }
        public Phone[] Phone { get; set; }
        public string EmailAddress { get; set; }
    }
}