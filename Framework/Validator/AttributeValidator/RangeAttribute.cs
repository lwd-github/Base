namespace Validator.AttributeValidator
{
    /// <summary>
    /// 数值范围特性
    /// </summary>
    public class RangeAttribute : BaseValidator
    {
        private decimal Min = 0;
        private decimal Max = 0;

        public RangeAttribute(decimal min, decimal max)
        {
            Min = min;
            Max = max;
        }

        public override bool Valitate(object value)
        {
            return value != null
                && decimal.TryParse(value.ToString(), out decimal lValue)
                && lValue >= Min
                && lValue <= Max;
        }
    }
}
