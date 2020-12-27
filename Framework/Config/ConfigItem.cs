﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public class ConfigItem
    {
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
