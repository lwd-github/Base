using Common;
using Common.Extension;
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

        [HttpGet("/enum/{name}")]
        public IEnumerable<NameValue<int>> Get(string name)
        {
            if (dicEnum.ContainsKey(name))
            {
                return dicEnum[name].GetItems();
            }

            throw new Exception("未匹配到对应的枚举信息");
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
