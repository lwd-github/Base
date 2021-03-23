using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Config
{
    public class Auth
    {
        public List<KeySecret> KeySecrets { get; set; }
    }

    public class KeySecret
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Secret { get; set; }
    }
}
