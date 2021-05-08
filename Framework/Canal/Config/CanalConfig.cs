using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Canal.Config
{
    /// <summary>
    /// Canal配置
    /// </summary>
    public class CanalConfig
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// Canal的默认端口：11111
        /// </summary>
        public ushort Port { get; set; } = 11111;

        /// <summary>
        /// Canal配置的destination，默认为：example
        /// </summary>
        public string Destination { get; set; } = "example";

        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 订阅，Filter是一种过滤规则，通过该规则的表数据变更才会传递过来
        /// 允许所有数据 .*\\..*
        /// 允许某个库数据 库名\\..*
        /// 允许某些表 库名.表名,库名.表名
        /// 多个规则组合使用：canal\\..*,mysql.test1,mysql.test2 (逗号分隔)；注意：此过滤条件只针对row模式的数据有效
        /// </summary>
        public string Subscribe { get; set; } = ".*\\..*";
    }
}
