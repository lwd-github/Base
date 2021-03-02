using Cache.Local;
using Common.Results;
using CommonModel.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        readonly string cacheKey = CacheKeys.SmsCodeKey("13650701695");

        public CacheController(ILocalCache localCache)
        {
            _localCache = localCache;
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
            _localCache.Set(cacheKey, value);
        }
    }
}
