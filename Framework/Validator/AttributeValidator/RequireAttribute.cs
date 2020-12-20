using Validator.AttributeValidator;

namespace Validator.AttributeValidator
{
    /// <summary>
    /// 必填特性
    /// </summary>
    public class RequireAttribute : BaseValidator
    {
        public override bool Valitate(object value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
