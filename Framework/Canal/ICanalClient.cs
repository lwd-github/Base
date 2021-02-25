
using System;

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
        void Receive(Action<CanalMessage<string>> action);
    }
}
