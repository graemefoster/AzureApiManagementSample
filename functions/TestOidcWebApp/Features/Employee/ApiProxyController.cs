using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestOidcWebApp.EmployeeApi;

namespace TestOidcWebApp.Features.Employee
{
    [Authorize]
    [Route("/[controller]")]
    public class ApiProxyController : Controller
    {
        private readonly EmployeeApis _client;

        public ApiProxyController(EmployeeApis client)
        {
            _client = client;
        }

        [Route("{**catchAll}")]
        [HttpGet]
        public async Task<IActionResult> Get(string catchAll)
        {
            //going to just proxy this through to APIm with the JWT to allow us to use, e.g. react on the front-end
            return base.Ok(await _client.CallApiRawAsync(catchAll));
        }
        
    }
}