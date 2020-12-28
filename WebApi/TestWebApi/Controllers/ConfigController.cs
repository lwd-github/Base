using Common.Results;
using CommonModel.Config;
using CommonService.Config;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet("/config/auth")]
        public Result<Auth> Get()
        {
            return new Result<Auth>
            {
                Data = SysConfig<Auth>.Instance
            };
        }
    }
}
