﻿using Common.Extension;

namespace Validator.AttributeValidator
{
    /// <summary>
    /// 数值范围特性
    /// </summary>
    public class RangeAttribute : BaseValidator
    {
        readonly double Min;
        readonly double Max;

        public RangeAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public override bool Valitate(object value)
        {
            return value.IsNotNull()
                && double.TryParse(value.ToString(), out double pValue)
                && pValue >= Min
                && pValue <= Max;
        }
    }
}
