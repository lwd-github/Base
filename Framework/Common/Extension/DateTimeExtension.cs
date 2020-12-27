using System;

namespace Common.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 判断是否为最小值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMinValue(this DateTime input)
        {
            return input == DateTime.MinValue;
        }

        /// <summary>
        /// 判断是否为空值或者最小值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrMinValue(this DateTime? input)
        {
            return (input == null || input.Value == DateTime.MinValue);
        }

        /// <summary>
        /// 判断是否非空并且非最小值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNullOrMinValue(this DateTime? input)
        {
            return !input.IsNullOrMinValue();
        }
    }
}
