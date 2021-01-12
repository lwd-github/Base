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
        /// 创建生产者（用于工作队列）
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        IMQProducer CreateProducer(string queue);

        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">routingKey</param>
        /// <returns></returns>
        IMQProducer CreateProducer(Exchange exchange, string routingKey = "");

        /// <summary>
        /// 创建消费者（用于工作队列）
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns></returns>
        IMQConsumer CreateConsumer(string queue);

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <param name="exchange">交换机</param>
        /// <param name="routingKey">路由Key</param>
        /// <param name="deadLetter">死信设置</param>
        /// <returns></returns>
        IMQConsumer CreateConsumer(string queue, Exchange exchange, string routingKey = "", DeadLetter deadLetter = null);

        /// <summary>
        /// 删除队列
        /// </summary>
        /// <param name="queue">队列名</param>
        /// <returns>返回删除队列期间清除的消息数</returns>
        uint QueueDelete(string queue);

        /// <summary>
        /// 删除交换机
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        void ExchangeDelete(string exchange);
    }
}
