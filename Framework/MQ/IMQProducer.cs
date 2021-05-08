using System;

namespace Framework.MQ
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
        /// <param name="obj">消息对象</param>
        /// <param name="expiration">消息有效期（单位：秒）</param>
        void Send<T>(T obj, uint expiration = 0);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="expiration">消息有效期（单位：秒）</param>
        void Send(string message, uint expiration = 0);
    }
}
