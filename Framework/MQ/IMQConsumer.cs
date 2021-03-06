﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.MQ
{
    /// <summary>
    /// 消息队列消费者
    /// </summary>
    public interface IMQConsumer
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        void Receive(Action<string> action);

        /// <summary>
        /// 拉取消息
        /// </summary>
        /// <param name="action"></param>
        void Pull(Action<string> action);

        /// <summary>
        /// 停止接收消息
        /// </summary>
        void Stop();
    }
}
