using System;
using System.Collections.Generic;

namespace Canal
{
    /// <summary>
    /// Canal客户端
    /// </summary>
    public interface ICanalClient
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="action"></param>
        void Receive(Action<List<CanalMessage>> action);

    }
}
