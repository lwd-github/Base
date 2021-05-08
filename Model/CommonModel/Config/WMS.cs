using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Config
{
    public class WMS
    {
        /// <summary>
        /// 11
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 22
        /// </summary>
        public WMSSecurity Security { get; set;}
    }

    public class WMSSecurity
    {
        /// <summary>
        /// 33
        /// </summary>
        public string PublicKey { get; set; }
    }
}
