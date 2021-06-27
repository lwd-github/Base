using System;
using System.Collections.Generic;
using System.Text;

namespace Enumeration.System
{
    /// <summary>
    /// 响应结果编码
    /// </summary>
    public enum EResultCode
    {
        /// <summary>
        /// (未知)错误
        /// </summary>
        Error = 0,

        /// <summary>
        /// 成功
        /// </summary>
        OK = 1,

        /// <summary>
        /// 验证错误
        /// </summary>
        ValidationError = 2,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 3,

        /// <summary>
        /// 服务不可用
        /// </summary>
        ServiceUnavailable = 500
    }
}
