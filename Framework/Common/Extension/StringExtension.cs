using System.Text.RegularExpressions;

namespace Framework.Common.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符串是否为空字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串是否不为空字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串是否为null或者空白字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否不为null或者空白字符串
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="options">枚举值的一个按位组合，默认值为指定不区分大小写的匹配</param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return Regex.IsMatch(str, pattern, options);
        }

        /// <summary>
        /// 判断参数是否为Email
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return str.IsMatch("^[a-z_0-9.-]{1,64}@([a-z0-9-]{1,200}.){1,5}[a-z]{1,6}$");
        }

        /// <summary>
        /// 判断参数是否为电话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(this string str)
        {
            return
                str.IsMatch(
                    @"^\d{11}$");
        }

        /// <summary>
        /// 将json序列化成对象
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="json">json格式的字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
