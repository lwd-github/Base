using System;

namespace Common.Exceptions
{
    /// <summary>
    /// 验证异常
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {

        }
    }
}
