using Validator.Extension;

namespace Validator.AttributeValidator
{
    /// <summary>
    /// 字符长度特性
    /// </summary>
    public class LengthAttribute : BaseValidator
    {
        private int MaxLength = 0; //最大长度

        public LengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public override bool Valitate(object value)
        {
            if (value == null) return true;

            //如果为字符串
            string str = value as string;
            if (str.IsNull()) return true;

            return str.Length <= MaxLength;
        }
    }
}
