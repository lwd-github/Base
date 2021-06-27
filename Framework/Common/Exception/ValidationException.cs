using System;

namespace Framework.Common.Exception
{
    /// <summary>
    /// 验证异常
    /// </summary>
    public class ValidationException : System.Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">异常编码</param>
        /// <param name="message">异常信息</param>
        public ValidationException(int code, string message) : base(message)
        {
            base.HResult = code;
        }
    }
}
