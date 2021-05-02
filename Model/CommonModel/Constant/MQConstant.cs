using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Constant
{
    public static class MQConstant
    {
        /// <summary>
        /// 死信交换机
        /// </summary>
        public const string DeadLetterExchange = "Sys1-DeadLetter-Exchange";

        public const string IOCAssemblies = "CommonService.dll;TestService.dll;Cache.dll";
    }
}
