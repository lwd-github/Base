using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Config
{
    public class WMS
    {
        public string Url { get; set; }

        public WMSSecurity Security { get; set;}
    }

    public class WMSSecurity
    {
        public string PublicKey { get; set; }
    }
}
