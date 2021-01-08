using CommonService.Config;
using Microsoft.Extensions.Hosting;
using MQ;
using MQ.Config;
using MQ.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQConsumerTest
{
    public class MQConsumerService : IHostedService
    {
        readonly SysConfig<MQConfig> _sysConfig;
        readonly IMQContext _mqContext;

        public MQConsumerService()
        {
            _sysConfig = new SysConfig<MQConfig>();
            _mqContext = new RabbitMQContext(_sysConfig.Value);
        }

        private void Start()
        {
            var consumer = _mqContext.CreateConsumer("Test_123",new Exchange { Name= "Bps-PaySuccessed", Type = ExchangeType.Fanout });
            consumer.Receive(MyReceive);
        }

        private void MyReceive(string msg)
        {
            Console.WriteLine($"MQ消息：{msg}");
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
