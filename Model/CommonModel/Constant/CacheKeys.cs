using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel.Constant
{
    /// <summary>
    /// 缓存Key
    /// </summary>
    public class CacheKeys
    {
        /// <summary>
        /// 系统标签
        /// </summary>
        private static string Prefix = "erp";

        /// <summary>
        /// 短信验证码key
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string SmsCodeKey(string phone) => $"{Prefix}_smscode_{phone}";
    }
}
