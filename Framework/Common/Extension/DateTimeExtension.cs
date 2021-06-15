using System;

namespace Framework.Common.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime UnixTimeStampStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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

        /// <summary>  
        /// 将DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="input">时间</param>  
        /// <returns></returns>  
        public static long ConvertToSecond(this DateTime input)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long t = (input.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            //return t;
            return (long)(input.ToUniversalTime() - UnixTimeStampStart).TotalSeconds; //将时间转化为秒
        }

        /// <summary>
        /// 时间戳转为格式时间
        /// </summary>
        /// <param name="timeStamp">时间戳，单位秒</param>
        /// <returns></returns>        
        public static DateTime ConvertToDateTime(this long timeStamp)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //long lTime = long.Parse(timeStamp + "0000");
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow);

            //DateTime time = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(timeStamp), TimeZoneInfo.Local);
            return UnixTimeStampStart.AddSeconds(timeStamp).ToLocalTime();
        }
    }
}
