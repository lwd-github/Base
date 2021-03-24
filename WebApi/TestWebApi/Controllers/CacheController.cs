using Cache.Local;
using Common.Results;
using DTO.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQ;
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
        readonly IMQContext _mqContext;
        readonly string cacheKey = CacheKeys.SmsCodeKey("13650701695");

        public CacheController(ILocalCache localCache, IMQContext mqContext)
        {
            _localCache = localCache;
            _mqContext = mqContext;
        }

        [HttpGet("/cache")]
        public Result<string> Get()
        {
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
