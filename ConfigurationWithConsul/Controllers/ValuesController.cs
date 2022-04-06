using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationWithConsul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase

    {
        private readonly IConfiguration _configuration;
        private readonly AppSettings _options;
        private readonly AppSettings _optionsSnapshot;

        public ValuesController(IConfiguration configuration, IOptions<AppSettings> options, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this._configuration = configuration;
            this._options = options.Value;
            this._optionsSnapshot = optionsSnapshot.Value;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { _configuration["DemoAppSettings:Key1"], _options.Key1, _options.Key2, _optionsSnapshot.Key2, _configuration["Serilog:Using:0"] };
        }
    }
}
