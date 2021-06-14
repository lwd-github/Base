using Framework.Common.Extension;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Security.Cryptography
{
    public class JwtHelper
    {
        /// <summary>
        /// 获取JWT的载荷
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public static Payload GetPayload(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm alg = new HMACSHA256Algorithm();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
            return decoder.Decode(token).ToObject<Payload>();
        }
    }


    /// <summary>
    /// 载荷
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// 定义在什么时间之前该jwt是不可用的
        /// </summary>
        public long nbf { get; set; }

        /// <summary>
        ///  jwt的过期时间，是一个 unix 时间戳
        /// </summary>
        public long exp { get; set; }

        /// <summary>
        /// 该 jwt 的签发者
        /// </summary>
        public string iss { get; set; }

        /// <summary>
        /// 接收该 jwt 的一方
        /// </summary>
        public string aud { get; set; }

        public string client_id { get; set; }

        public string sub { get; set; }

        public long auth_time { get; set; }

        public string idp { get; set; }

        /// <summary>
        /// jwt的唯一标识，主要用作一次性token，避免重放攻击
        /// </summary>
        public string jti { get; set; }

        /// <summary>
        /// jwt的签发时间
        /// </summary>
        public string iat { get; set; }

        public List<string> scope { get; set; }

        public List<string> amr { get; set; }
    }
}
