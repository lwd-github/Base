using Common.Results;
using Config;
using DTO.Config;
using Microsoft.AspNetCore.Mvc;
using MQ;
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
        readonly IMQContext _mqContext;
        
        public ConfigController(IMQContext mqContext)
        {
            _mqContext = mqContext;
        }

        [HttpGet("/config/auth")]
        public Result<Auth> Get()
        {
            return new Result<Auth>
            {
                Data = ConfigAgent.Value<Auth>()
            };
        }


        [HttpGet("/mq/message")]
        public Result GetMQ()
        {
            //工作队列
            var consumer1 = _mqContext.CreateConsumer("Test_Queue_1");
            consumer1.Receive(MyReceive1);
            return new Result();
        }

        private void MyReceive1(string msg)
        {
            Console.WriteLine($"第1个消费者获取的MQ消息：{msg}");
            //throw new Exception("测试队列消费异常");
        }

    }
}
