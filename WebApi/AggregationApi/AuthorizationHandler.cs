using DTO.Constant;
using Framework.Cache.Redis;
using Framework.Common.Results;
using Framework.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Framework.Common.Extension;

namespace AggregationApi
{
    /// <summary>
    /// 权限处理器
    /// </summary>
    public class AuthorizationHandler : IAuthorizationFilter
    {
        readonly RedisCache _redisCache;

        public AuthorizationHandler(RedisCache redisCache)
        {
            _redisCache = redisCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attrs = GetAttributes(context.ActionDescriptor as ControllerActionDescriptor);

            if (attrs.OfType<AuthorizeAttribute>().Any())
            {
                var accessToken = context.HttpContext.GetTokenAsync("Bearer", "access_token").Result;
                var payload = JwtHelper.GetPayload(accessToken);
                var userId = payload.sub;

                var cacheValue = _redisCache.Hash.Get(CacheKeys.AccessTokenKey, userId);

                if (cacheValue.IsNullOrWhiteSpace() || cacheValue != accessToken)
                {
                    context.Result = new JsonResult(new Result
                    {
                        Status = false,
                        Code = (int)HttpStatusCode.Unauthorized,
                        Message = "未授权"
                    });
                }
            }
        }


        /// <summary>
        /// 获取Action所有特性，包括当前控制器
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private IEnumerable<object> GetAttributes(ControllerActionDescriptor descriptor)
        {
            var allAttrs = new List<object>();
            // action特性
            var actionAttrs = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
            if (actionAttrs != null)
                allAttrs.AddRange(actionAttrs);

            // controller特性
            var controllerAttrs = descriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true);
            if (controllerAttrs != null)
                allAttrs.AddRange(controllerAttrs);

            return allAttrs;
        }
    }
}
