using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Framework.Common.Extension;
using System.IO;
using System.Net.Http.Headers;
using Baidu.Aip.Ocr;
using System.Web;

namespace XUnitTest.BaiduAI
{
    public class CharacterRecognitionTest
    {
        //https://cloud.baidu.com/product/ocr
        //https://www.cnblogs.com/jerryqi/p/11592676.html

        [Fact]
        public void RecognitionTest()
        {
            Recognition();

            Recognition1();

            Recognition2();

            Recognition3();
        }


        private string GetAccessToken()
        {
            string responseJson;
            var url = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient httpClient = new HttpClient();

            //MultipartFormDataContent => multipart / form-data
            //FormUrlEncodedContent => application / x-www-form-urlencoded
            //StringContent => application / json等
            //StreamContent => binary

            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "grant_type", "client_credentials" },
                { "client_id", "wd2RatemhCiO7M5qfq9DXoAp" },
                { "client_secret", "eEw8ZGc83YeTLpyXroojh8dUpvaaqC74" }
            });
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var Result = httpClient.PostAsync(url, content).Result;

            if (Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseJson = Result.Content.ReadAsStringAsync().Result.ToObject<TokenInfo>().access_token;
            }
            else
            {
                throw new Exception("Request Url is " + url + " Response is " + Result.StatusCode);
            }

            return responseJson;
        }


        private string Recognition()
        {
            string responseJson;
            var url = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic";

            var accessToken = GetAccessToken();

            HttpClient httpClient = new HttpClient();

            //MultipartFormDataContent => multipart / form-data
            //FormUrlEncodedContent => application / x-www-form-urlencoded
            //StringContent => application / json等
            //StreamContent => binary

            //文字图片的base64编码后的结果
            byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\过磅单图片数据-02.jpg");
            string base64 = Convert.ToBase64String(bytes);
            var image = HttpUtility.UrlEncode(base64);

            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("access_token",accessToken),
                new KeyValuePair<string, string>("image",image),
                new KeyValuePair<string, string>("language_type","CHN_ENG")
            };

            var data = new List<string>();
            list.ForEach(item =>
            {
                data.Add($"{item.Key}={item.Value}");
            });

            var content = new StringContent(string.Join("&", data.ToArray()));

            var Result = httpClient.PostAsync(url, content).Result;

            if (Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseJson = Result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception("Request Url is " + url + " Response is " + Result.StatusCode);
            }

            return responseJson;
        }


        private void Recognition1()
        {
            string apiKey = "wd2RatemhCiO7M5qfq9DXoAp";
            string secretKey = "eEw8ZGc83YeTLpyXroojh8dUpvaaqC74";

            Ocr client = new Ocr(apiKey, secretKey)
            {
                Timeout = 30000 //超时时间30秒
            };
            //文字图片的base64编码后的结果
            byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\过磅单图片数据-02.jpg");
            var jobj = client.AccurateBasic(bytes);
            var result = client.AccurateBasic(bytes).ToString().ToObject<GeneralBasicResult>();
        }


        private string Recognition2()
        {
            string responseJson;
            var url = "https://aip.baidubce.com/rest/2.0/solution/v1/iocr/recognise";

            var accessToken = GetAccessToken();

            HttpClient httpClient = new HttpClient();

            //MultipartFormDataContent => multipart / form-data
            //FormUrlEncodedContent => application / x-www-form-urlencoded
            //StringContent => application / json等
            //StreamContent => binary

            //文字图片的base64编码后的结果
            byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\过磅单图片数据-02.jpg");
            string base64 = Convert.ToBase64String(bytes);
            var image = HttpUtility.UrlEncode(base64);

            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("access_token",accessToken),
                new KeyValuePair<string, string>("image",image),
                new KeyValuePair<string, string>("Content-Type","application/x-www-form-urlencoded"),
                new KeyValuePair<string, string>("templateSign","537edc3c01fdcafc5da4a4e4a6881d2a")
            };

            var data = new List<string>();
            list.ForEach(item =>
            {
                data.Add($"{item.Key}={item.Value}");
            });

            var content = new StringContent(string.Join("&", data.ToArray()));

            var Result = httpClient.PostAsync(url, content).Result;

            if (Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseJson = Result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                throw new Exception("Request Url is " + url + " Response is " + Result.StatusCode);
            }

            return responseJson;
        }


        private string Recognition3()
        {
            string apiKey = "wd2RatemhCiO7M5qfq9DXoAp";
            string secretKey = "eEw8ZGc83YeTLpyXroojh8dUpvaaqC74";

            Ocr client = new Ocr(apiKey, secretKey)
            {
                Timeout = 30000 //超时时间30秒
            };
            //文字图片的base64编码后的结果
            byte[] bytes = File.ReadAllBytes(@"C:\Users\future\Desktop\过磅单图片数据-02.jpg");
            var dic = new Dictionary<string, object>
                        {
                            { "templateSign", "537edc3c01fdcafc5da4a4e4a6881d2a" }
                        };
            var result = client.Custom(bytes, dic).ToString();
            return result;
        }
    }


    public class TokenInfo
    {
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
        public string session_key { get; set; }
        public string access_token { get; set; }
        public string scope { get; set; }
        public string session_secret { get; set; }
    }


    public class GeneralBasicResult
    {
        public List<WordsResult> words_result { get; set; }
    }

    public class WordsResult
    {
        public string words { get; set; }
    }
}
