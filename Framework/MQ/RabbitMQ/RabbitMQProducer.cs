using Common.Extension;
using MQ.Config;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.RabbitMQ
{
    public class RabbitMQProducer : IMQProducer,IDisposable
    {
        readonly RabbitMQContext _context;
        readonly string _queue;
        readonly Exchange _exchange;
        IConnection _connection;

        internal RabbitMQProducer(RabbitMQContext context, string queue):this(context, queue,null)
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

            using (var session = _connection.CreateModel())
            {
                if (_exchange.IsNotNull()) 
                {
                    session.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);
                }

                session.QueueDeclare(_queue, true, false, false, null);

                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = session.CreateBasicProperties();
                properties.Persistent = true;
                session.BasicPublish(_exchange?.Name?? string.Empty, this._queue, properties, body);
            }
        }
    }
}
