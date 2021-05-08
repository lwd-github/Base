using Framework.Cache.Local;
using Framework.Cache.Redis;
using Framework.Common.Results;
using DTO.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Framework.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.Controllers
{
    [ApiController]
    public class CacheController : ControllerBase
    {
        readonly ILocalCache _localCache;
        readonly RedisCache _redisCache;
        readonly IMQContext _mqContext;
        readonly string cacheKey = CacheKeys.SmsCodeKey("13650701695");

        public CacheController(ILocalCache localCache, RedisCache redisCache, IMQContext mqContext)
        {
            _localCache = localCache;
            _redisCache = redisCache;
            _mqContext = mqContext;
        }

        [HttpGet("/cache")]
        public Result<string> Get()
        {
            var result = _redisCache.Get("redisCache_key3");

            return new Result<string>
            {
                Data = _localCache.Get(cacheKey)
            };
        }

        [HttpPost("/cache")]
        public void Post(string value)
        {
            _localCache.Set(cacheKey, value, 20);
        }

        [HttpDelete("/cache")]
        public void Delete()
        {
            _localCache.Remove(cacheKey);
        }
    }
}
