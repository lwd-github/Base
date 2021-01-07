using Common.Extension;
using MQ.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ消息队列消费者
    /// </summary>
    public class RabbitMQConsumer : IMQConsumer, IDisposable
    {
        readonly RabbitMQContext _context;
        readonly string _queue;
        readonly Exchange _exchange;
        readonly string _routingKey;
        IConnection _connection;

        internal RabbitMQConsumer(RabbitMQContext context, string queue) : this(context, queue, null)
        {

        }

        internal RabbitMQConsumer(RabbitMQContext context, string queue, Exchange exchange, string routingKey = "")
        {
            _context = context;
            _queue = queue;
            _exchange = exchange;
            _routingKey = routingKey;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        public void Receive(Action<string> action)
        {
            if (!_context.IsConnected(_connection))
            {
                _connection = _context.CreateConnection();
            }

            var channel = _connection.CreateModel();

            //创建队列
            channel.QueueDeclare(_queue, true, false, false, null);

            if (_exchange.IsNotNull())
            {
                //创建交接机
                channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                //绑定交换机和队列
                channel.QueueBind(queue: _queue,
                                  exchange: _exchange.Name,
                                  routingKey: _routingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray(); //消息主体
                    var message = Encoding.UTF8.GetString(body);
                    action(message);
                    (sender as IBasicConsumer)?.Model?.BasicNack(ea.DeliveryTag, false, false);
                }
                catch
                {
                    (sender as IBasicConsumer)?.Model?.BasicNack(ea.DeliveryTag, false, true);
                    throw;
                }
            };
            channel.BasicConsume(_queue, false, "", false, false, null, consumer);
        }

        /// <summary>
        /// 停止接收消息
        /// </summary>
        public void Stop()
        {
            _connection?.Dispose();
        }
    }
}
