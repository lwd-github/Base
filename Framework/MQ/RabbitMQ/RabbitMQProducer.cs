using Common.Extension;
using MQ.Config;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ消息队列生产者
    /// </summary>
    public class RabbitMQProducer : IMQProducer, IDisposable
    {
        readonly RabbitMQContext _context;
        readonly bool _isUnnamedQueue; //队列是否未命名：true=未命名，false=已命名
        readonly string _queue;
        readonly Exchange _exchange;
        IConnection _connection;

        internal RabbitMQProducer(RabbitMQContext context, string queue) : this(context, queue, null)
        {

        }

        internal RabbitMQProducer(RabbitMQContext context, string queue, Exchange exchange)
        {
            _context = context;
            _queue = queue ?? string.Empty;
            _isUnnamedQueue = _queue.IsNullOrEmpty();
            _exchange = exchange;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">消息对象</param>
        /// <param name="expiration">消息有效期（单位：秒）</param>
        public void Send<T>(T obj, uint expiration = 0)
        {
            Send(obj.ToJson(), expiration);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="expiration">消息有效期（单位：秒）</param>
        public void Send(string message, uint expiration = 0)
        {
            if (!_context.IsConnected(_connection))
            {
                _connection = _context.CreateConnection();
            }

            using (var channel = _connection.CreateModel())
            {
                if (!_isUnnamedQueue)
                {
                    //创建队列：如果队列名为空字符串，会生成随机名称的队列
                    channel.QueueDeclare(_queue, true, false, false, null);
                }

                if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
                {
                    //创建交接机
                    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                    if (!_isUnnamedQueue)
                    {
                        //绑定交换机和队列
                        channel.QueueBind(queue: _queue,
                                  exchange: _exchange.Name,
                                  routingKey: _queue);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
                    }
                }

                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                if (expiration > 0)
                {
                    properties.Expiration = $"{expiration * 1000}";
                }

                channel.BasicPublish(_exchange?.Name ?? string.Empty, this._queue, properties, body);
            }
        }
    }
}
