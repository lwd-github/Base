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
        readonly bool _isHasdQueue = false; //是否有队列：true=有，false=否
        readonly string _queue;
        readonly Exchange _exchange;
        readonly string _routingKey;
        readonly IConnection _connection;


        /*
         * 主题交换器（Topic Exchange）
         * 发送到主题交换器的消息不能有任意的routing key，必须是由点号分开的一串单词，这些单词可以是任意的，但通常是与消息相关的一些特征。比如以下是几个有效的routing key："nyse.vmw", "quick.orange.rabbit"，routing key的单词可以有很多，最大限制是255 bytes。
         * 使用特定路由键发送的消息将被发送到所有使用匹配绑定键绑定的队列，然而，绑定键有两个特殊的情况，如下：
         * 【* 表示匹配任意一个单词】
         * 【# 表示匹配任意一个或多个单词】
         */

        internal RabbitMQProducer(RabbitMQContext context, string queue) : this(context, null, "")
        {
            _queue = queue ?? string.Empty;
            _isHasdQueue = _queue.IsNotNullOrEmpty();

            if (_routingKey.IsNullOrEmpty())
            {
                _routingKey = _queue;
            }
        }

        internal RabbitMQProducer(RabbitMQContext context, Exchange exchange, string routingKey = "")
        {
            _context = context;
            _exchange = exchange;
            _routingKey = routingKey;
            _connection = _context.CreateConnection(); //创建连接
            DeclareExchangeAndQueue();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        /// <summary>
        /// 定义交换机和队列
        /// </summary>
        private void DeclareExchangeAndQueue()
        {
            using (var channel = _connection.CreateModel())
            {
                if (_isHasdQueue)
                {
                    //创建队列：如果队列名为空字符串，会生成随机名称的队列
                    channel.QueueDeclare(_queue, true, false, false, null);
                }

                if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
                {
                    //创建交接机
                    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                    if (_isHasdQueue)
                    {
                        //绑定交换机和队列
                        channel.QueueBind(queue: _queue,
                                  exchange: _exchange.Name,
                                  routingKey: _routingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
                    }
                }
            }
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
            //if (!_context.IsConnected(_connection))
            //{
            //    _connection = _context.CreateConnection();
            //}

            using (var channel = _connection.CreateModel())
            {
                //if (!_isUnnamedQueue)
                //{
                //    //创建队列：如果队列名为空字符串，会生成随机名称的队列
                //    channel.QueueDeclare(_queue, true, false, false, null);
                //}

                //if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
                //{
                //    //创建交接机
                //    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                //    if (!_isUnnamedQueue)
                //    {
                //        //绑定交换机和队列
                //        channel.QueueBind(queue: _queue,
                //                  exchange: _exchange.Name,
                //                  routingKey: _routingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
                //    }
                //}

                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                if (expiration > 0)
                {
                    properties.Expiration = $"{expiration * 1000}";
                }

                channel.BasicPublish(_exchange?.Name ?? string.Empty, _routingKey, properties, body);
            }
        }
    }
}
