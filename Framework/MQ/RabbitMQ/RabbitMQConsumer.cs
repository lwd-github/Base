using Framework.Common.Extension;
using Framework.MQ.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.MQ.RabbitMQ
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
        readonly bool _isSetDeadLetter = false; //是否已设置死信队列：true=已设置，false=未设置
        readonly bool _isHasDeadLetterQueue = false; //是否有死信队列：true=有，false=否
        readonly DeadLetter _deadLetter;
        readonly IConnection _connection;

        internal RabbitMQConsumer(RabbitMQContext context, string queue) : this(context, queue, null)
        {

        }

        internal RabbitMQConsumer(RabbitMQContext context, string queue, Exchange exchange, string routingKey = "", DeadLetter deadLetter = null)
        {
            if (queue.IsNullOrEmpty()) throw new ArgumentNullException(nameof(queue));
            _context = context;
            _queue = queue;
            _exchange = exchange;
            _routingKey = routingKey;
            _deadLetter = deadLetter;
            _isSetDeadLetter = _deadLetter.IsNotNull() && _deadLetter.Exchange.IsNotNull() && _deadLetter.Exchange.Name.IsNotNullOrEmpty();
            _isHasDeadLetterQueue = _deadLetter.IsNotNull() && _deadLetter.Queue.IsNotNullOrEmpty();
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
                IDictionary<string, object> keys = null;

                if (_isSetDeadLetter)
                {
                    keys = new Dictionary<string, object>()
                    {
                        //{"x-expires",1800000}, //过期时间，参数值以毫秒为单位
                        //{ "x-max-length", 100 }, //队列的最大长度
                        { "x-dead-letter-exchange", _deadLetter.Exchange.Name },
                        { "x-dead-letter-routing-key",_deadLetter.RoutingKey}
                    };

                    //创建死信交换机
                    channel.ExchangeDeclare(_deadLetter.Exchange.Name, _deadLetter.Exchange.Type, true, false, null);

                    if (_isHasDeadLetterQueue)
                    {
                        //创建死信队列
                        channel.QueueDeclare(_deadLetter.Queue, true, false, false, null);

                        //绑定死信交换机和死信队列
                        channel.QueueBind(queue: _deadLetter.Queue,
                                          exchange: _deadLetter.Exchange.Name,
                                          routingKey: _deadLetter.RoutingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
                    }
                }

                //创建队列：如果队列名为空字符串，会生成随机名称的队列
                //绑定死信交换机
                channel.QueueDeclare(_queue, true, false, false, keys);

                if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
                {
                    //创建交接机
                    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                    //绑定交换机和队列
                    channel.QueueBind(queue: _queue,
                                      exchange: _exchange.Name,
                                      routingKey: _routingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
                }
            }
        }

        /// <summary>
        /// 演示代码
        /// </summary>
        private void DeclareExchangeAndQueueTest()
        {
            using (var channel = _connection.CreateModel())
            {
                IDictionary<string, object> keys = null;

                //死信交换机和路由key
                keys = new Dictionary<string, object>()
                {
                    { "x-dead-letter-exchange", _deadLetter.Exchange.Name },
                    { "x-dead-letter-routing-key",_deadLetter.RoutingKey}
                };

                //创建业务队列，并为业务队列配置死信交换机和路由key
                channel.QueueDeclare(_queue, true, false, false, keys);

                //创建业务交换机
                channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

                //绑定业务队列和业务交换机
                channel.QueueBind(queue: _queue,
                                  exchange: _exchange.Name,
                                  routingKey: _routingKey);

                //创建死信队列
                channel.QueueDeclare(_deadLetter.Queue, true, false, false, null);

                //创建死信交换机
                channel.ExchangeDeclare(_deadLetter.Exchange.Name, _deadLetter.Exchange.Type, true, false, null);

                //绑定死信交换机和死信队列
                channel.QueueBind(queue: _deadLetter.Queue,
                                  exchange: _deadLetter.Exchange.Name,
                                  routingKey: _deadLetter.RoutingKey);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        public void Receive(Action<string> action)
        {
            //if (!_context.IsConnected(_connection))
            //{
            //    _connection = _context.CreateConnection();
            //}

            var channel = _connection.CreateModel();

            ////创建队列：如果队列名为空字符串，会生成随机名称的队列
            //channel.QueueDeclare(_queue, true, false, false, null);

            //if (_exchange.IsNotNull() && _exchange.Name.IsNotNull())
            //{
            //    //创建交接机
            //    channel.ExchangeDeclare(_exchange.Name, _exchange.Type, true, false, null);

            //    //绑定交换机和队列
            //    channel.QueueBind(queue: _queue,
            //                      exchange: _exchange.Name,
            //                      routingKey: _routingKey);  //bingding key（即参数的routingKey）的意义取决于exchange的类型。fanout类型的exchange会忽略这个值。
            //}

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
                    //(sender as IBasicConsumer)?.Model?.BasicNack(ea.DeliveryTag, false, false);
                    (sender as IBasicConsumer)?.Model?.BasicAck(ea.DeliveryTag, false);
                }
                catch
                {
                    //requeue=true：队列会重新推送
                    //(sender as IBasicConsumer)?.Model?.BasicNack(ea.DeliveryTag, false, true);
                    (sender as IBasicConsumer)?.Model?.BasicNack(ea.DeliveryTag, false, !_isSetDeadLetter);
                    throw;
                }
            };
            channel.BasicConsume(_queue, false, "", false, false, null, consumer);
        }

        /// <summary>
        /// 拉取消息
        /// </summary>
        /// <param name="action"></param>
        public void Pull(Action<string> action)
        {
            using (var channel = _connection.CreateModel())
            {
                //拉取消息
                BasicGetResult response = channel.BasicGet(_queue, false);

                if (response.IsNotNull())
                {
                    var body = response.Body.ToArray(); //消息主体
                    var message = Encoding.UTF8.GetString(body);
                    action(message);
                    channel.BasicAck(response.DeliveryTag, false);
                }
            }
        }

        /// <summary>
        /// 停止接收消息
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

    }
}
