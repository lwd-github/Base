using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Config
{
    public class ConfigItem
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SysCode { get; set; }

        /// <summary>
        /// 配置项键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 配置项值：json格式
        /// </summary>
        public string Value { get; set; }
    }
}
