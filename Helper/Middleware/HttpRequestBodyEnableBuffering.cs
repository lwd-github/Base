using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Middleware
{
    public class HttpRequestBodyEnableBuffering
    {
        public static readonly string Key = "EnableBodyBuffer";
        private readonly RequestDelegate _next;

        public HttpRequestBodyEnableBuffering(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await Task.Run(() =>
            {
                context.Items.Add(Key, null);
                context.Request.EnableBuffering();
            });
            await _next.Invoke(context);
        }
    }
}
