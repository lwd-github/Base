using System;
using System.Collections.Generic;
using System.Text;

namespace Map
{
    public class LocationOutputForGaoDe
    {
        /// <summary>
        /// 返回结果状态值：0 表示请求失败；1 表示请求成功
        /// </summary>
        public ushort Status { get; set; }

        /// <summary>
        /// 返回状态说明：当 status 为 0 时，info 会返回具体错误原因，否则返回“OK”
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public string Infocode { get; set; }

        /// <summary>
        /// 返回结果数目
        /// </summary>
        public ushort Count { get; set; }

        /// <summary>
        /// 地理编码信息列表
        /// </summary>
        public IList<GeoCodeForGaoDe> GeoCodes { get; set; }
    }
}
