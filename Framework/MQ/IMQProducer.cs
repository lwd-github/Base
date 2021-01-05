using System;

namespace MQ
{
    /// <summary>
    /// 消息队列生产者
    /// </summary>
    public interface IMQProducer
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        void Send<T>(T item);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);
    }
}
