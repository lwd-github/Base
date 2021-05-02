using Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestService;

namespace TestWebApi.Controllers
{
    [ApiController]
    public class TestServiceController : ControllerBase
    {

        private IEnumerable <ITest> _test;

        public TestServiceController(IEnumerable<ITest> test)
        {
            _test = test;
        }


        [HttpGet("/test")]
        public Result<string> Get()
        {
            var value = _test.FirstOrDefault(t => t.Type == 2).GetValue();
            return new Result<string> { Status = true, Data = value };
        }
    }
}
