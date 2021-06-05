using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Ftp.Config
{
    public class FtpConfig
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public ushort Port { get; set; } = 21;

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
