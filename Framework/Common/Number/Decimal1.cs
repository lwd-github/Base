using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Number
{
    /// <summary>
    /// 1位小数
    /// </summary>
    public struct Decimal1
    {
        decimal _val;

        public Decimal1(decimal value)
        {
            _val = Math.Round(value, 1, MidpointRounding.AwayFromZero);
        }


        public static implicit operator Decimal1(decimal value) => new Decimal1(value);
        //public static implicit operator Decimal1(int value) => value;
        //public static implicit operator Decimal1(long value) => value;
        //public static implicit operator Decimal2(Decimal1 value) => new Decimal2(value);
        public static implicit operator decimal(Decimal1 value) => value._val;

        public static Decimal1 operator +(Decimal1 d1, Decimal1 d2) => new Decimal1(d1._val + d2._val);

        public static Decimal1 operator -(Decimal1 d1, Decimal1 d2) => new Decimal1(d1._val - d2._val);

        public static Decimal1 operator *(Decimal1 d1, Decimal1 d2) => new Decimal1(d1._val * d2._val);

        public static Decimal1 operator /(Decimal1 d1, Decimal1 d2) => new Decimal1(d1._val / d2._val);

        public static Decimal1 operator %(Decimal1 d1, Decimal1 d2) => new Decimal1(d1._val % d2._val);

        public static bool operator >(Decimal1 d1, Decimal1 d2) => d1._val > d2._val;

        public static bool operator <(Decimal1 d1, Decimal1 d2) => d1._val < d2._val;

        public static bool operator >=(Decimal1 d1, Decimal1 d2) => d1._val >= d2._val;

        public static bool operator <=(Decimal1 d1, Decimal1 d2) => d1._val <= d2._val;

        public static bool operator ==(Decimal1 d1, Decimal1 d2) => d1._val == d2._val;

        public static bool operator !=(Decimal1 d1, Decimal1 d2) => d1._val != d2._val;
    }
}
