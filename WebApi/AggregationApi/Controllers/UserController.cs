using DTO.User;
using Enumeration.System;
using Framework.Common.Results;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AggregationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public UserController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public Result<IdentityDto> Login([FromBody] LoginInput input)
        {
            //var httpClient = new HttpClient();
            var httpClient = _clientFactory.CreateClient(EWebApiName.UserApi.ToString());

            // 1、获取IdentityServer接口文档
            //DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:5010").Result;
            DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync().Result;
            if (discoveryDocument.IsError)
            {
                Console.WriteLine($"[DiscoveryDocumentResponse Error]: {discoveryDocument.Error}");
            }

            // 2、根据用户名和密码建立token
            TokenResponse tokenResponse = httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client1",
                ClientSecret = "secret",
                GrantType = "password",
                UserName = input.UserName, //"mail@qq.com",
                Password = input.UserPassword //"123"
            }).Result;

            // 3、返回AccessToken
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error + "," + tokenResponse.Raw);
            }

            // 4、获取用户信息
            UserInfoResponse userInfoResponse = httpClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = discoveryDocument.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            }).Result;

            // 5、返回UserDto信息
            IdentityDto identityDto = new IdentityDto();
            identityDto.UserId = userInfoResponse.Json.TryGetString("sub");
            identityDto.UserName = input.UserName;
            identityDto.AccessToken = tokenResponse.AccessToken;
            identityDto.ExpiresIn = tokenResponse.ExpiresIn;

            return new Result<IdentityDto> { Status = true, Data = identityDto };
        }


        [HttpPost("Logout")]
        [Authorize]
        public Result<string> Logout([FromBody] IdentityDto input)
        {
            var httpClient = new HttpClient();

            // 1、获取IdentityServer接口文档
            DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:5010").Result;
            if (discoveryDocument.IsError)
            {
                Console.WriteLine($"[DiscoveryDocumentResponse Error]: {discoveryDocument.Error}");
            }

            var result = httpClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = discoveryDocument.RevocationEndpoint,
                ClientId = "client1",
                ClientSecret = "secret",
                Token = input.AccessToken
                //TokenTypeHint = "refresh_token"
            }).Result;

            if (result.IsError)
            {
                Console.WriteLine(result.Error);
                return new Result<string> { Status = false, Data = "Error" };
            }
            else
            {
                Console.WriteLine(result.HttpErrorReason);
                return new Result<string> { Status = true, Data = result.HttpErrorReason };
            }
        }


        [HttpGet]
        [Authorize]
        public Result<string> Get()
        {
            return new Result<string> { Status = true, Data = "OK!" };
        }
    }
}
