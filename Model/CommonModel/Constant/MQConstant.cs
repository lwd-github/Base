using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Constant
{
    /// <summary>
    /// MQ常量
    /// </summary>
    public static class MQConstant
    {
        /// <summary>
        /// 死信交换机
        /// </summary>
        public static string DeadLetterExchange = $"{SystemConstant.SystemName}-DeadLetter-Exchange";
    }
}
