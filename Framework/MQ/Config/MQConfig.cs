using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.Config
{
    /// <summary>
    /// 消息队列配置信息
    /// </summary>
    public class MQConfig
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port { get; set; } = 5672;

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
    }
}
