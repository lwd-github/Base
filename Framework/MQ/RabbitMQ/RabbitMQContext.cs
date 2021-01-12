using Common.Extension;
using MQ.Config;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ消息队列操作的上下文（建议使用单例）
    /// </summary>
    public class RabbitMQContext : IMQContext
    {
        readonly MQConfig _config;
        IConnectionFactory _factory;

        public RabbitMQContext(MQConfig config)
        {
            _config = config;
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (_factory.IsNull())
            {
                var factory = new ConnectionFactory();
                factory.HostName = _config.Host;
                factory.Port = _config.Port;
                factory.UserName = _config.User ?? string.Empty;
                factory.Password = _config.Password ?? string.Empty;
                factory.VirtualHost = _config.VirtualHost ?? "/";
                _factory = factory;
            }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public IConnection CreateConnection()
        {
            return _factory.CreateConnection();
        }

        /// <summary>
        /// 判断连接是否可用
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public bool IsConnected(IConnection connection)
        {
            return connection.IsNotNull() && connection.IsOpen;
        }

        /// <summary>
        /// 创建消费者（用于工作队列）
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        public IMQConsumer CreateConsumer(string queue)
        {
            return new RabbitMQConsumer(this, queue);
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">路由Key</param>
        /// <param name="deadLetter">死信设置</param>
        /// <returns></returns>
        public IMQConsumer CreateConsumer(string queue, Exchange exchange, string routingKey = "", DeadLetter deadLetter = null)
        {
            return new RabbitMQConsumer(this, queue, exchange, routingKey, deadLetter);
        }

        /// <summary>
        /// 创建生产者（用于工作队列）
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        public IMQProducer CreateProducer(string queue)
        {
            return new RabbitMQProducer(this, queue);
        }

        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">routingKey</param>
        /// <returns></returns>
        public IMQProducer CreateProducer(Exchange exchange, string routingKey = "")
        {
            return new RabbitMQProducer(this, exchange, routingKey);
        }

        /// <summary>
        /// 删除队列
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns>返回删除队列期间清除的消息数</returns>
        public uint QueueDelete(string queue)
        {
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    return channel.QueueDelete(queue);
                }
            }
        }

        /// <summary>
        /// 删除交换机
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        public void ExchangeDelete(string exchange)
        {
            using (var connection = CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDelete(exchange);
                }
            }
        }
    }
}
