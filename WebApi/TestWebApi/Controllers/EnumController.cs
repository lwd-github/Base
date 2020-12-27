using Common;
using Common.Exceptions;
using Common.Extension;
using Common.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        readonly Dictionary<string, Type> dicEnum = new Dictionary<string, Type>();

        public EnumController()
        {
            dicEnum["OrderStatus"] = typeof(OrderStatus);
        }

        /// <summary>
        /// 获取枚举信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("/enum/{name}")]
        public Result<IEnumerable<NameValue<int>>> Get(string name)
        {
            var result = new Result<IEnumerable<NameValue<int>>>();

            if (dicEnum.ContainsKey(name))
            {
                result.Status = true;
                result.Data = dicEnum[name].GetItems();
                return result;
            }

            throw new ValidationException("未匹配到对应的枚举信息");
        }
    }


    public enum OrderStatus
    {
        [Description("订单等待付款")]
        等待付款 = 0,
        待发货 = 1,
        待收货 = 2,
        已收货 = 3
    }
}
