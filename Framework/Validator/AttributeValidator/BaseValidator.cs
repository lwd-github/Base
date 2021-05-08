using System;

namespace Framework.Validator.AttributeValidator
{
    /// <summary>
    /// 特性验证器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseValidator : Attribute
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public abstract bool Valitate(object value);
    }
}
