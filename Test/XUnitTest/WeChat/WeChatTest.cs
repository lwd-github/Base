using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Framework.Common.Extension;
using System.Security.Cryptography;
using System.IO;

namespace XUnitTest.WeChat
{
    public class WeChatTest : BaseTest
    {
        string _appid = "wx64e25d399e9d7ddc";
        string _secret = "8ce98aca7cd641d2bffccfec64e35a35";

        [Fact]
        public void GetAccessTokenTest()
        {
            var result = GetAccessToken();
        }


        [Fact]
        public void GetCode2SessionTest()
        {
            var grant_type = "authorization_code";
            var JsCode = "123"; //通过wx.login() 接口获得临时登录凭证 code
            var url = $"https://api.weixin.qq.com/sns/jscode2session?appid={_appid}&secret={_secret}&js_code={JsCode}&grant_type={grant_type}";

            using (var client = new HttpClient())
            {
                var response = client.GetStringAsync(url).Result; //40029：code 无效
            }
        }


        [Fact]
        public void GetWxaCodeTest()
        {
            //https://developers.weixin.qq.com/miniprogram/dev/api-backend/open-api/qr-code/wxacode.getUnlimited.html

            //获取小程序码，适用于需要的码数量极多的业务场景。通过该接口生成的小程序码，永久有效，数量暂无限制。
            var accessToken = GetAccessToken();
            var url = $"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={accessToken.access_token}";

            using (var client = new HttpClient())
            {
                var request = new WxaCodeUnlimitInput { scene = "a=1" };

                var requestContent = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, requestContent).Result;

                if (response.Content.Headers.ContentType.MediaType.Contains("application/json; charset=UTF-8"))
                {
                    var result = response.Content.ReadAsStringAsync().Result.ToObject<WeChatErrOutput>();

                    if (result.errcode.IsNotNullOrWhiteSpace() && result.errcode != "0") { throw new Exception(result.errmsg); }
                }

                using (MemoryStream stream = new MemoryStream(response.Content.ReadAsByteArrayAsync().Result))
                {
                    File.WriteAllBytes(@"C:\Users\future\Desktop\WxaCode.png", stream.ToArray());
                }
            }
        }


        public WeChatAccessTokenOutput GetAccessToken()
        {
            var grant_type = "client_credential";
            var url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type={grant_type}&appid={_appid}&secret={_secret}";

            using (var client = new HttpClient())
            {
                var content = client.GetStringAsync(url).Result;
                var result = content.ToObject<WeChatAccessTokenOutput>();

                if (result.errcode.IsNotNullOrWhiteSpace() && result.errcode != "0") { throw new Exception(result.errmsg); }

                return result;
            }
        }


        /// <summary>
        /// 微信解密算法
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        private string Decrypt(string encryptedData, string iv, string sessionKey)
        {
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();

            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;

            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;

            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);
            return result;
        }
    }


    public class WeChatAccessTokenOutput: WeChatErrOutput
    {
        /// <summary>
        /// 凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值
        /// </summary>
        public ulong expires_in { get; set; }
    }


    public class WeChatErrOutput
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }


    public class WxaCodeUnlimitInput
    { 
        public string scene { get; set; }
    }

}
