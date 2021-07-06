using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace sample_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeadersController : ControllerBase
    {

        [HttpGet]
        public IDictionary<string, string> Get()
        {
            return base.Request.Headers.ToDictionary(h => h.Key, h => string.Join(',',  h.Value));
        }
    }
}
