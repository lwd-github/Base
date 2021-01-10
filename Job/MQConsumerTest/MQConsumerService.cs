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
            //工作队列
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1");
            //consumer1.Receive(MyReceive1);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_1");
            //consumer2.Receive(MyReceive2);

            //发布/订阅
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1", new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //consumer1.Receive(MyReceive1);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //consumer2.Receive(MyReceive2);

            //RoutingKey（单播）
            var consumer1 = _mqContext.CreateConsumer("Test_Queue_1", new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct });
            consumer1.Receive(MyReceive1);

            var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct });
            consumer2.Receive(MyReceive2);
        }

        private void MyReceive1(string msg)
        {
            Console.WriteLine($"第1个消费者获取的MQ消息：{msg}");
        }

        private void MyReceive2(string msg)
        {
            Console.WriteLine($"第2个消费者获取的MQ消息：{msg}");
        }

        private void QueueDelete()
        {
            //var i = _mqContext.QueueDelete("Test_Queue_1");
            //Console.WriteLine($"删除队列时，清除的消息数：{i}");

            //var j = _mqContext.QueueDelete("Test_Queue_2");
            //Console.WriteLine($"删除队列时，清除的消息数：{j}");

            //_mqContext.ExchangeDelete("Test_Exchange_1");
            //_mqContext.ExchangeDelete("Test_Exchange_2");
            //Console.Read();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(QueueDelete);
            //return null;
            //throw new NotImplementedException();
        }
    }
}
