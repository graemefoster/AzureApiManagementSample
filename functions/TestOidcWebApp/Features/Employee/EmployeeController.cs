using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestOidcWebApp.EmployeeApi;
using TestOidcWebApp.Features.Shared;

namespace TestOidcWebApp.Features.Employee
{
    [Authorize]
    [Route("/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmployeeApis _client;

        public EmployeeController(ILogger<EmployeeController> logger, EmployeeApis client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string employmentNumber)
        {
            var vm = new EmployeeModel
            {
                EmploymentNumber = employmentNumber,
                Summary = await _client.GetSummary(employmentNumber)
            };
            return View(vm);
        }

        [HttpGet]
        [Route("Details/{employmentNumber}")]
        public async Task<IActionResult> Details(string employmentNumber)
        {
            var vm = new EmployeeModel
            {
                EmploymentNumber = employmentNumber,
                Summary = await _client.GetSummary(employmentNumber)
            };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}