using Framework.Common.Extension;
using System;

namespace Framework.Validator.AttributeValidator
{
    /// <summary>
    /// 必填特性
    /// </summary>
    public class RequireAttribute : BaseValidator
    {
        public override bool Valitate(object value)
        {
            if (value.IsNull()) return false;

            //如果为字符串
            string str = value as string;
            if(str != null && str.IsNullOrWhiteSpace()) return false;

            //如果为时间类型
            if (value is DateTime && ((DateTime)value).IsMinValue()) return false;

            return true;
        }
    }
}
