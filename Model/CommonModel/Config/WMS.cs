using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel.Config
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
