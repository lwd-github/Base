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
        IConnection _connection;

        internal RabbitMQConsumer(RabbitMQContext context, string queue) : this(context, queue, null)
        {

        }

        internal RabbitMQConsumer(RabbitMQContext context, string queue, Exchange exchange)
        {
            if (queue.IsNullOrEmpty()) throw new ArgumentNullException(nameof(queue));
            _context = context;
            _queue = queue;
            _exchange = exchange;
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

            //创建队列：如果队列名为空字符串，会生成随机名称的队列
            channel.QueueDeclare(_queue, true, false, false, null);

            if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
            {
                //创建交接机
                channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                //绑定交换机和队列
                channel.QueueBind(queue: _queue,
                                  exchange: _exchange.Name,
                                  routingKey: _queue);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
            }

            //这个设置会告诉RabbitMQ 每次给Workder只分配一个task，只有当task执行完了，才分发下一个任务
            /*
             * 可以通过设置prefetchCount来限制Queue每次发送给每个消费者的消息数，
             * 比如我们设置prefetchCount=1，则Queue每次给每个消费者发送一条消息；
             * 消费者处理完这条消息后Queue会再给该消费者发送一条消息。
             * 
             * prefetchCount 的默认值为0，即没有限制，队列会将所有消息尽快发给消费者
             */
            channel.BasicQos(0, 1, false);

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
