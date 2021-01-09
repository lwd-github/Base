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
        readonly SysConfig<MQConfig> _sysConfig;
        readonly IMQContext _mqContext;

        public MQProducerService()
        {
            _sysConfig = new SysConfig<MQConfig>();
            _mqContext = new RabbitMQContext(_sysConfig.Value);
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
            //var producer = _mqContext.CreateProducer("", new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //producer.Send($"MQ发布/订阅{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");

            //RoutingKey（单播）
            var producer = _mqContext.CreateProducer("Test_Queue_2", new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct });
            producer.Send($"MQ单播{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")}");
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
