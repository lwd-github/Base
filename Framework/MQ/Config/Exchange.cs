using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ.Config
{
    public class Exchange
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public string Type { get; set; }
    }
}
