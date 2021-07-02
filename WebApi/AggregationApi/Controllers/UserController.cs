using DTO.Constant;
using DTO.Results;
using DTO.User;
using Enumeration.System;
using Framework.Cache.Redis;
using Framework.Common.Results;
using Framework.Common.Extension;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Framework.Validator;
using Framework.Common.Exception;

namespace AggregationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IHttpClientFactory _clientFactory;
        readonly HttpClient _userApiClient;
        readonly RedisCache _redisCache;

        public UserController(IHttpClientFactory clientFactory, RedisCache redisCache)
        {
            _clientFactory = clientFactory;
            _userApiClient = _clientFactory.CreateClient(EWebApiName.UserApi.ToString());
            _redisCache = redisCache;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
        public Result<IdentityDto> Login([FromBody] LoginInput input)
        {
            //var httpClient = new HttpClient();

            // 1、获取IdentityServer接口文档
            //DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:5010").Result;
            DiscoveryDocumentResponse discoveryDocument = _userApiClient.GetDiscoveryDocumentAsync().Result;
            
            //if (discoveryDocument.IsError)
            //{
            //    Console.WriteLine($"[DiscoveryDocumentResponse Error]: {discoveryDocument.Error}");
            //}

            Verify.If(discoveryDocument.IsError, new ValidationException((int)EResultCode.ServiceUnavailable, discoveryDocument.Error));

            // 2、根据用户名和密码建立token
            TokenResponse tokenResponse = _userApiClient.RequestPasswordTokenAsync(new PasswordTokenRequest()
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
            UserInfoResponse userInfoResponse = _userApiClient.GetUserInfoAsync(new UserInfoRequest()
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
            identityDto.TokenType = tokenResponse.TokenType;
            identityDto.RefreshToken = tokenResponse.RefreshToken;

            //6、写入缓存
            _redisCache.Hash.Set(CacheKeys.AccessTokenKey, identityDto.UserId, identityDto.AccessToken);

            return new ResultSuccess<IdentityDto> { Data = identityDto };
        }


        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("RefreshToken")]
        public Result<IdentityDto> RefreshToken([FromBody] RefreshTokenInput input)
        {
            //var httpClient = new HttpClient();

            // 1、获取IdentityServer接口文档
            //DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:5010").Result;
            DiscoveryDocumentResponse discoveryDocument = _userApiClient.GetDiscoveryDocumentAsync().Result;
            if (discoveryDocument.IsError)
            {
                Console.WriteLine($"[DiscoveryDocumentResponse Error]: {discoveryDocument.Error}");
            }

            // 2、根据用户名和密码建立token
            TokenResponse tokenResponse = _userApiClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client1",
                ClientSecret = "secret",
                GrantType = "refresh_token",
                //UserName = input.UserName, //"mail@qq.com",
                //Password = input.UserPassword //"123"
                RefreshToken = input.RefreshToken
            }).Result;

            // 3、返回AccessToken
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error + "," + tokenResponse.Raw);
            }

            // 4、获取用户信息
            UserInfoResponse userInfoResponse = _userApiClient.GetUserInfoAsync(new UserInfoRequest()
            {
                Address = discoveryDocument.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            }).Result;

            // 5、返回UserDto信息
            IdentityDto identityDto = new IdentityDto();
            identityDto.UserId = userInfoResponse.Json.TryGetString("sub");
            //identityDto.UserName = input.UserName;
            identityDto.AccessToken = tokenResponse.AccessToken;
            identityDto.ExpiresIn = tokenResponse.ExpiresIn;
            identityDto.TokenType = tokenResponse.TokenType;
            identityDto.RefreshToken = tokenResponse.RefreshToken;

            return new ResultSuccess<IdentityDto> { Data = identityDto };
        }


        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("Logout")]
        [Authorize]
        public Result<string> Logout([FromBody] IdentityDto input)
        {

            _redisCache.Hash.Remove(CacheKeys.AccessTokenKey, input.UserId);

            //var httpClient = new HttpClient();

            // 1、获取IdentityServer接口文档
            //DiscoveryDocumentResponse discoveryDocument = httpClient.GetDiscoveryDocumentAsync("https://localhost:5010").Result;
            DiscoveryDocumentResponse discoveryDocument = _userApiClient.GetDiscoveryDocumentAsync().Result;
            if (discoveryDocument.IsError)
            {
                Console.WriteLine($"[DiscoveryDocumentResponse Error]: {discoveryDocument.Error}");
            }

            //令牌注销只针对引用令牌（reference token）
            var result = _userApiClient.RevokeTokenAsync(new TokenRevocationRequest
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
                return new Result<string> { Code = EResultCode.Error.ToInt(),  Data = result.Error };
            }
            else
            {
                Console.WriteLine(result.HttpErrorReason);
                return new ResultSuccess<string> { Data = result.HttpErrorReason };
            }
        }


        [HttpGet]
        [Authorize]
        public Result<string> Get()
        {
            var accessToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result;
            DiscoveryDocumentResponse discoveryDocument = _userApiClient.GetDiscoveryDocumentAsync().Result;

            //var result = _userApiClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            //{
            //    Address = discoveryDocument.IntrospectionEndpoint,

            //    ClientId = "client1",　　　　//资源名称
            //    ClientSecret = "secret",　　 //资源私钥
            //    Token = accessToken
            //}).Result;

            //if (result.IsError)
            //{
            //    Console.WriteLine(result.Error);
            //    throw new Exception(result.Error);
            //}
            //else
            //{
            //    if (result.IsActive)　　//返回看IsActive->true表示有效,false表示无效
            //    {
            //        result.Claims.ToList().ForEach(c => Console.WriteLine("{0}: {1}",
            //            c.Type, c.Value));
            //    }
            //    else
            //    {
            //        Console.WriteLine("token is not active");
            //        throw new Exception("token is not active");
            //    }
            //}

            return new ResultSuccess<string> { Data = "OK!" };
        }
    }
}
