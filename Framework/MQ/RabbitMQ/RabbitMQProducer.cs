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
        readonly string _queue;
        readonly Exchange _exchange;
        IConnection _connection;

        internal RabbitMQProducer(RabbitMQContext context, string queue) : this(context, queue, null)
        {

        }

        internal RabbitMQProducer(RabbitMQContext context, string queue, Exchange exchange)
        {
            _context = context;
            _queue = queue;
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
        /// <param name="obj"></param>
        public void Send<T>(T obj)
        {
            Send(obj.ToJson());
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if (!_context.IsConnected(_connection))
            {
                _connection = _context.CreateConnection();
            }

            using (var channel = _connection.CreateModel())
            {
                if(_queue.IsNotNull())
                { 
                    //创建队列
                    channel.QueueDeclare(_queue, true, false, false, null);
                }

                if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
                {
                    //创建交接机
                    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);
                }

                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(_exchange?.Name?? string.Empty, this._queue?? string.Empty, properties, body);
            }
        }
    }
}
