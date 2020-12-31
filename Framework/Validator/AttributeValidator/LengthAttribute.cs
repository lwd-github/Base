
namespace Validator.AttributeValidator
{
    /// <summary>
    /// 字符长度特性
    /// </summary>
    public class LengthAttribute : BaseValidator
    {
        readonly int MinLength = 0; //最少长度
        readonly int MaxLength = 0; //最大长度

        public LengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        public LengthAttribute(int minLength, int maxLength)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public override bool Valitate(object value)
        {
            //如果为字符串
            string str = value as string;
            int length = str?.Length ?? 0;

            return length >= MinLength && length <= MaxLength;
        }
    }
}
