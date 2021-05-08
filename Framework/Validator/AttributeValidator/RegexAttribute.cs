using Framework.Common.Extension;

namespace Framework.Validator.AttributeValidator
{
    /// <summary>
    /// 正则表达式特性
    /// </summary>
    public class RegexAttribute : BaseValidator
    {
        readonly string Pattern; //正则表达式

        public RegexAttribute(string pattern)
        {
            Pattern = pattern;
            ErrorMessage = "邮箱格式不正确";
        }

        public override bool Valitate(object value)
        {
            //如果为字符串
            string str = value as string;
            if (str.IsNull() || str.IsNullOrEmpty()) return true;
            return str.IsMatch(Pattern);
        }
    }
}
