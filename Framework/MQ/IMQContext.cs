using MQ.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ
{
    /// <summary>
    /// 消息队列操作的上下文（建议使用单例）
    /// </summary>
    public interface IMQContext
    {
        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        IMQProducer CreateProducer(string queue);

        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="exchange">交换机</param>
        /// <returns></returns>
        IMQProducer CreateProducer(string queue, Exchange exchange);

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        IMQConsumer CreateConsumer(string queue);

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        IMQConsumer CreateConsumer(string queue, Exchange exchange, string routingKey = "");
    }
}
