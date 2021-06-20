using DTO.User;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Common.Extension;
using Framework.Common.Results;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4;
using Framework.Security.Cryptography;

namespace MVCClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            #region token模式 
            {
                //// 1、生成AccessToken
                //// 1.1 客户端模式
                //// 1.2 客户端用户密码模式
                //// 1.3 客户端code状态码模式
                //string access_token = await GetAccessToken();
                IdentityDto dentityDto = new IdentityDto();
                var client = new HttpClient();
                //client.DefaultRequestHeaders.Add("ContentType", "application/json");
                HttpContent httpContent = new StringContent(new LoginInput { UserName= "mail@qq.com", UserPassword="123"}.ToJson());
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("https://localhost:5020/api/User/Login", httpContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Request Error, StatusCode is : {response.StatusCode}");
                }
                else
                {
                    var content = response.Content.ReadAsStringAsync().Result.ToObject<Result<IdentityDto>>();
                    dentityDto = content.Data;
                    Console.WriteLine($"AccessToken is : {dentityDto.AccessToken}");
                }

                // 2、使用AccessToken 进行资源访问
                string result = UseAccessToken(dentityDto.AccessToken).Result;

                result = RefreshToken(dentityDto.RefreshToken).Result;

                LogOut(dentityDto);

                result = UseAccessToken(dentityDto.AccessToken).Result;

                //// 3、响应结果到页面
                //ViewData.Add("Json", result);
            }
            #endregion

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        ///// <summary>
        ///// 1、生成token
        ///// </summary>
        ///// <returns></returns>
        //public static async Task<string> GetAccessToken()
        //{
        //    //1、建立连接
        //    HttpClient client = new HttpClient();
        //    DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync("http://localhost:5005");
        //    if (disco.IsError)
        //    {
        //        Console.WriteLine($"[DiscoveryDocumentResponse Error]: {disco.Error}");
        //    }
        //    // 1.1、通过客户端获取AccessToken
        //    //TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    //{
        //    //    Address = disco.TokenEndpoint, // 1、生成AccessToken中心
        //    //    ClientId = "client", // 2、客户端编号
        //    //    ClientSecret = "secret",// 3、客户端密码
        //    //    Scope = "TeamService" // 4、客户端需要访问的API
        //    //});

        //    // 1.2 通过客户端用户密码获取AccessToken
        //    //TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        //    //{
        //    //    Address = disco.TokenEndpoint,
        //    //    ClientId = "client-password",
        //    //    ClientSecret = "secret",
        //    //    Scope = "TeamService",
        //    //    UserName = "tony",
        //    //    Password = "123456"
        //    //});

        //    // 1.3 通过授权code获取AccessToken[需要进行登录]
        //    TokenResponse tokenResponse = await client.RequestAuthorizationCodeTokenAsync
        //        (new AuthorizationCodeTokenRequest
        //        {
        //            Address = disco.TokenEndpoint,
        //            ClientId = "client-code",
        //            ClientSecret = "secret",
        //            Code = "12",
        //            RedirectUri = "http://localhost:5005"

        //        });
        //    if (tokenResponse.IsError)
        //    {
        //        //ClientId 与 ClientSecret 错误，报错：invalid_client
        //        //Scope 错误，报错：invalid_scope
        //        //UserName 与 Password 错误，报错：invalid_grant
        //        string errorDesc = tokenResponse.ErrorDescription;
        //        if (string.IsNullOrEmpty(errorDesc)) errorDesc = "";
        //        if (errorDesc.Equals("invalid_username_or_password"))
        //        {
        //            Console.WriteLine("用户名或密码错误，请重新输入！");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"[TokenResponse Error]: {tokenResponse.Error}, [TokenResponse Error Description]: {errorDesc}");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Access Token: {tokenResponse.Json}");
        //        Console.WriteLine($"Access Token: {tokenResponse.RefreshToken}");
        //        Console.WriteLine($"Access Token: {tokenResponse.ExpiresIn}");
        //    }
        //    return tokenResponse.AccessToken;
        //}

        /// <summary>
        /// 2、使用token
        /// </summary>
        public async Task<string> UseAccessToken(string AccessToken)
        {
            HttpClient apiClient = new HttpClient();
            apiClient.SetBearerToken(AccessToken); // 1、设置token到请求头
            HttpResponseMessage response = await apiClient.GetAsync("https://localhost:5020/api/User");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Request Error, StatusCode is : {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                //Console.WriteLine($"Result: {JArray.Parse(content)}");

                // 3、输出结果到页面
                //return JArray.Parse(content).ToString();
            }

            var payload = JwtHelper.GetPayload(AccessToken);
            DateTime exp = payload.exp.ConvertToDateTime();

            var now = DateTime.Now;
            var sec = now.ConvertToSecond();
            return "";
        }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<string> RefreshToken(string refreshToken)
        {
            HttpClient apiClient = new HttpClient();
            //apiClient.SetBearerToken(AccessToken); // 1、设置token到请求头
            HttpContent httpContent = new StringContent(new RefreshTokenInput { RefreshToken = refreshToken }.ToJson());
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await apiClient.PostAsync("https://localhost:5020/api/User/RefreshToken", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Request Error, StatusCode is : {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();

                var payload = JwtHelper.GetPayload(content);

                Console.WriteLine(content);
                //Console.WriteLine($"Result: {JArray.Parse(content)}");

                // 3、输出结果到页面
                //return JArray.Parse(content).ToString();
            }

            return "";
        }


        public void LogOut(IdentityDto input)
        {
            HttpClient apiClient = new HttpClient();
            apiClient.SetBearerToken(input.AccessToken); // 1、设置token到请求头

            HttpContent httpContent = new StringContent(input.ToJson());
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = apiClient.PostAsync("https://localhost:5020/api/User/Logout", httpContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Request Error, StatusCode is : {response.StatusCode}");
            }
            else
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("");
                //Console.WriteLine($"Result: {JArray.Parse(content)}");

                // 3、输出结果到页面
                //return JArray.Parse(content).ToString();

                HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
            }
        }
    }
}
