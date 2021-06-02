﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Cache.Redis.Config
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port { get; set; } = 6379;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
