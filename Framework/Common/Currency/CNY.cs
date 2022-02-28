using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Common.Currency
{
    /// <summary>
    /// 人民币
    /// </summary>
    public struct CNY
    {
        decimal _val;
        public CNY(decimal value)
        {
            _val = value;
        }

        public CNY(int value)
        {
            _val = value;
        }

        public CNY(long value)
        {
            _val = value;
        }


        //decimal fmoney = 2100.126M;
        //var r = string.Format("{0:N5}", fmoney);

        //var cul = new CultureInfo("zh-CN");//中国大陆  
        //var _rmoney = fmoney.ToString("c", cul);

        //cul = new CultureInfo("zh-HK");//香港  
        //_rmoney = fmoney.ToString("c", cul);

        //    cul = new CultureInfo("en-US");//美国  
        //_rmoney = fmoney.ToString("c", cul);

        //    cul = new CultureInfo("en-GB");//英国  
        //_rmoney = fmoney.ToString("c", cul);

        public string ToFormatString() => string.Format("{0:C2}", _val);
        public override string ToString() => ToFormatString();


        public static implicit operator CNY(decimal value) => new CNY(value);
        public static implicit operator CNY(int value) => new CNY(value);
        public static implicit operator CNY(long value) => new CNY(value);

        public static implicit operator decimal(CNY value) => value._val;

        public static CNY operator +(CNY d1, CNY d2) => new CNY(d1._val + d2._val);

        public static CNY operator -(CNY d1, CNY d2) => new CNY(d1._val - d2._val);

        public static CNY operator *(CNY d1, CNY d2) => new CNY(d1._val * d2._val);

        public static CNY operator /(CNY d1, CNY d2) => new CNY(d1._val / d2._val);

        public static CNY operator %(CNY d1, CNY d2) => new CNY(d1._val % d2._val);

        public static bool operator >(CNY d1, CNY d2) => d1._val > d2._val;

        public static bool operator <(CNY d1, CNY d2) => d1._val < d2._val;

        public static bool operator >=(CNY d1, CNY d2) => d1._val >= d2._val;

        public static bool operator <=(CNY d1, CNY d2) => d1._val <= d2._val;

        public static bool operator ==(CNY d1, CNY d2) => d1._val == d2._val;

        public static bool operator !=(CNY d1, CNY d2) => d1._val != d2._val;
    }
}
