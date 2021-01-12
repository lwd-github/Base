﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.Config
{
    public class DeadLetter
    {
        /// <summary>
        /// 死信交换机
        /// </summary>
        public Exchange Exchange { get; set; }

        /// <summary>
        /// 死信路由key
        /// </summary>
        public string RoutingKey { get; set; } = string.Empty;
    }
}