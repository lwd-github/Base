using Common.Results;
using DTO.Config;
using DTO.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    //[ApiController]
    public class ModelBinderController : ControllerBase
    {
        [HttpPost("logininfo")]
        public Result<LoginInfo> Post([FromBody]LoginInfo input, [FromBody] WMS input1)
        {
            return new Result<LoginInfo>
            {
                Data = input
            };
        }
    }
}
