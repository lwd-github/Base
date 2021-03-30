using DTO.Constant;
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
using Common.Extension;
using DBModel;
using Canal;

namespace MQConsumerTest
{
    public class MQConsumerService : IHostedService
    {
        //readonly SysConfig _sysConfig;
        readonly IMQContext _mqContext;

        public MQConsumerService(IMQContext mqContext)
        {
            _mqContext = mqContext;
        }

        private void Start()
        {
            //工作队列
            var consumer1 = _mqContext.CreateConsumer("Test_Queue_1");
            consumer1.Receive(MyReceive1);

            var consumer2 = _mqContext.CreateConsumer("Test_Queue_1");
            consumer2.Receive(MyReceive2);

            //发布/订阅（广播）
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1", new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //consumer1.Receive(MyReceive1);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = "Test_Exchange_1", Type = ExchangeType.Fanout });
            //consumer2.Receive(MyReceive2);

            //RoutingKey（单播：可以有选择性地接收消息）
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1", new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct }, "Info");
            //consumer1.Receive(MyReceive1);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = "Test_Exchange_2", Type = ExchangeType.Direct }, "Erro");
            //consumer2.Receive(MyReceive2);

            //组播：自定义RoutingKey：系统名.消息类型.级别
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1", new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "*.Erro.#"); //获取所有系统的所有Erro类型的消息，能匹配到：Sys2.Erro.3；Sys1.Erro.1.1-1
            //consumer1.Receive(MyReceive1);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = "Test_Exchange_3", Type = ExchangeType.Topic }, "Sys1.Info.*"); //获取系统1，消息类型为Info，任意级别的消息，能匹配到：Sys1.Info.1，不能匹配：Sys1.Info.1.1-1
            //consumer2.Receive(MyReceive2);

            //测试死信队列
            //var consumer1 = _mqContext.CreateConsumer("Test_Queue_1",
            //    new Exchange { Name = "Test_Exchange_4", Type = ExchangeType.Fanout },
            //    deadLetter: new DeadLetter
            //    {
            //        Queue = "DeadLetter_Test_Queue_1", //在原有队列名加前缀：DeadLetter_
            //        Exchange = new Exchange { Name = MQConstant.DeadLetterExchange, Type = ExchangeType.Fanout }
            //    });
            //consumer1.Receive(MyReceive3);

            //var consumer2 = _mqContext.CreateConsumer("Test_Queue_2", new Exchange { Name = MQConstant.DeadLetterExchange, Type = ExchangeType.Fanout });
            //consumer2.Receive(MyReceive4);

            //var consumer3 = _mqContext.CreateConsumer("DeadLetter_Test_Queue_1");
            //consumer3.Pull(MyPull);

            //Canal消息
            //var tableName = "t1";
            //var exchangeName = $"Canal_{tableName}_Exchange";
            //var queueName = $"Canal_{tableName}_Queue";
            //var consumer4 = _mqContext.CreateConsumer(queueName, new Exchange { Name = exchangeName, Type = ExchangeType.Fanout });
            //consumer4.Receive(Receive<CanalMessage<t1>>);
        }

        private void MyReceive1(string msg)
        {
            Console.WriteLine($"第1个消费者获取的MQ消息：{msg}");
            //throw new Exception("测试队列消费异常");
        }

        private void MyReceive2(string msg)
        {
            Console.WriteLine($"第2个消费者获取的MQ消息：{msg}");
        }

        private void MyReceive3(string msg)
        {
            Console.WriteLine($"第3个消费者获取的MQ消息：{msg}");
            //throw new Exception("测试队列消费异常");
        }

        private void MyReceive4(string msg)
        {
            Console.WriteLine($"接收到死信队列消息：{msg}");
        }

        private void Receive<T>(string msg)
        {
            var result = msg.ToObject<T>();
            Console.WriteLine($"消息对象{msg}");
        }

        private void MyPull(string msg)
        {
            Console.WriteLine($"拉取死信队列消息：{msg}");
            //throw new Exception("测试队列消费异常");
            //处理死信队列，修复错误后将消息重新推送
            var producer = _mqContext.CreateProducer(new Exchange { Name = "Test_Exchange_4", Type = ExchangeType.Fanout });
            producer.Send(msg);
        }

        private void QueueDelete()
        {
            //var i = _mqContext.QueueDelete("Test_Queue_1");
            //Console.WriteLine($"删除队列时，清除的消息数：{i}");

            //var j = _mqContext.QueueDelete("Test_Queue_2");
            //Console.WriteLine($"删除队列时，清除的消息数：{j}");

            //var k = _mqContext.QueueDelete("DeadLetter_Test_Queue_1");
            //Console.WriteLine($"删除队列时，清除的消息数：{k}");

            //_mqContext.ExchangeDelete("Test_Exchange_1");
            //_mqContext.ExchangeDelete("Test_Exchange_2");
            //_mqContext.ExchangeDelete("Test_Exchange_3");
            //_mqContext.ExchangeDelete("Test_Exchange_4");
            //_mqContext.ExchangeDelete(MQConstant.DeadLetterExchange);
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
