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
            return value.ToString().Length <= MaxLength;
        }
    }
}
