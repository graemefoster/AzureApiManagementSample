namespace TestOidcWebApp.EmployeeApi
{
    public class Address
    {
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string[] AddressLine { get; set; }
    }
}