using Framework.Common.Results;
using DTO.Config;
using DTO.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    [ApiController] //注释掉这行，则允许Action多个参数
    public class ModelBinderController : ControllerBase
    {
        /// <summary>
        /// 数据绑定测试
        /// </summary>
        /// <param name="input1">参数1</param>
        /// <param name="input">参数2</param>
        /// <returns></returns>
        [HttpPost("logininfo")]
        public Result<LoginInfo> Post([FromBody] WMS input1, [FromHeader]LoginInfo input)
        {
            return new Result<LoginInfo>
            {
                Data = input
            };
        }

        //[HttpGet("v1/logininfo")]
        //public Result<LoginInfo> Get(WMS input1, LoginInfo input2)
        //{
        //    return null;
        //}
    }
}
