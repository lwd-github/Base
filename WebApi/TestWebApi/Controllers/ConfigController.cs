using Common.Results;
using DTO.Config;
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
        readonly SysConfig _sysConfig;
        readonly BusinessConfig _businessConfig;

        public ConfigController(SysConfig sysConfig, BusinessConfig businessConfig)
        {
            _sysConfig = sysConfig;
            _businessConfig = businessConfig;
        }

        [HttpGet("/config/auth")]
        public Result<Auth> Get()
        {
            return new Result<Auth>
            {
                Data = _sysConfig.Value<Auth>()
            };
        }

        [HttpGet("/config/wms")]
        public Result<WMS> GetWMS()
        {
            return new Result<WMS>
            {
                Data = _businessConfig.Value<WMS>()
            };
        }
    }
}
