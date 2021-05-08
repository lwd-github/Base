using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Map.GaoDe
{
    public class GeoCode
    {
        /// <summary>
        /// 结构化地址信息：省份＋城市＋区县＋城镇＋乡村＋街道＋门牌号码
        /// </summary>
        public dynamic Formatted_Address { get; set; }

        /// <summary>
        /// 国家：国内地址默认返回中国
        /// </summary>
        public dynamic Country { get; set; }

        /// <summary>
        /// 地址所在的省份名
        /// </summary>
        public dynamic Province { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        public dynamic CityCode { get; set; }

        /// <summary>
        /// 地址所在的城市名
        /// </summary>
        public dynamic City { get; set; }

        /// <summary>
        /// 地址所在的区
        /// </summary>
        public dynamic District { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public dynamic Street { get; set; }

        /// <summary>
        /// 门牌
        /// </summary>
        public dynamic Number { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        public dynamic ADcode { get; set; }

        /// <summary>
        /// 坐标点:经度,纬度
        /// </summary>
        public dynamic Location { get; set; }

        /// <summary>
        /// 匹配级别
        /// </summary>
        public dynamic Level { get; set; }
    }
}
