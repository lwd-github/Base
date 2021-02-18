using CommonService.Config;
using Microsoft.Extensions.Hosting;
using MQ;
using MQ.Config;
using MQ.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQProducerTest
{
    public class MQProducerService : IHostedService
    {
        readonly SysConfig _sysConfig;
        readonly IMQContext _mqContext;

        public MQProducerService(SysConfig sysConfig, IMQContext mqContext)
        {
            _sysConfig = sysConfig;
            _mqContext = mqContext;
        }

        private void Start()
        {
            //工作队列
            //var producer = _mqContext.CreateProducer("Test_Queue_1");

            //for (int i = 0; i < 20; i++)
            //{
            //    producer.Send($"第{i}个MQ消息", uint.Parse(i.ToString()));
            //}

            //发布/订阅（广播）
            //var producer = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //producer.Send($"MQ广播{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //RoutingKey（单播）
            //var producer1 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct }, "Info");
            //producer1.Send($"MQ单播{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //var producer2 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct }, "Erro");
            //producer2.Send($"MQ单播{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //组播：自定义RoutingKey：系统名.消息类型.级别
            //var producer1 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "Sys1.Info.1");
            //producer1.Send($"MQ组播Sys1.Info.1 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //var producer2 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "Sys1.Info.1.1-1");
            //producer2.Send($"MQ组播Sys1.Info.1.1-1 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //var producer3 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "Sys1.Erro.1.1-1");
            //producer3.Send($"MQ组播Sys1.Erro.1.1-1 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //var producer4 = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "Sys2.Erro.3");
            //producer4.Send($"MQ组播Sys2.Erro.3 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //测试死信队列
            var producer = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_4", Type = ExchangeType.Fanout });
            producer.Send($"MQ广播{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
