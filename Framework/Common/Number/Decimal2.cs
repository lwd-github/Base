using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Number
{
    /// <summary>
    /// 2位小数
    /// </summary>
    public struct Decimal2
    {
        decimal _val;

        public Decimal2(decimal value)
        {
            _val = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }


        public static implicit operator Decimal2(decimal value) => new Decimal2(value);
        //public static implicit operator Decimal2(int value) => value;
        //public static implicit operator Decimal2(long value) => value;
        public static implicit operator Decimal2(Decimal1 value) => new Decimal2(value);
        public static implicit operator decimal(Decimal2 value) => value._val;

        public static Decimal2 operator +(Decimal2 d1, Decimal2 d2) => new Decimal2(d1._val + d2._val);

        public static Decimal2 operator -(Decimal2 d1, Decimal2 d2) => new Decimal2(d1._val - d2._val);

        public static Decimal2 operator *(Decimal2 d1, Decimal2 d2) => new Decimal2(d1._val * d2._val);

        public static Decimal2 operator /(Decimal2 d1, Decimal2 d2) => new Decimal2(d1._val / d2._val);

        public static Decimal2 operator %(Decimal2 d1, Decimal2 d2) => new Decimal2(d1._val % d2._val);

        public static bool operator >(Decimal2 d1, Decimal2 d2) => d1._val > d2._val;

        public static bool operator <(Decimal2 d1, Decimal2 d2) => d1._val < d2._val;

        public static bool operator >=(Decimal2 d1, Decimal2 d2) => d1._val >= d2._val;

        public static bool operator <=(Decimal2 d1, Decimal2 d2) => d1._val <= d2._val;

        public static bool operator ==(Decimal2 d1, Decimal2 d2) => d1._val == d2._val;

        public static bool operator !=(Decimal2 d1, Decimal2 d2) => d1._val != d2._val;
    }
}
