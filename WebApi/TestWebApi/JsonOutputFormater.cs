using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWebApi
{
    public class JsonOutputFormater : IOutputFormatter, IApiResponseTypeMetadataProvider
    {
        public JsonOutputFormater()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(CONTENT_TYPE));
        }

        public MediaTypeCollection SupportedMediaTypes { get; } = new MediaTypeCollection();

        protected const string CONTENT_TYPE = "application/json";


        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return context.HttpContext.Request.ContentType == null
                || context.HttpContext.Request.ContentType.Contains("multipart/form-data")
                || context.HttpContext.Request.ContentType.Contains("application/x-www-form-urlencoded")
                || context.HttpContext.Request.ContentType.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Contains(CONTENT_TYPE);

        }

        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;
            response.ContentType = CONTENT_TYPE;

            if (context.Object == null)
            {
                // 192 好像在 Response.Body 中表示 null
                response.Body.WriteByte(192);
                return Task.CompletedTask;
            }

            using (var writer = context.WriterFactory(response.Body, Encoding.UTF8))
            {
                // 使用 Jil 序列化
                JsonSerializer.SerializeToWriter(context.Object, context.ObjectType, writer);
                return Task.CompletedTask;
            }
        }

        public IReadOnlyList<string> GetSupportedContentTypes(string contentType, Type objectType)
        {
            return SupportedMediaTypes;
        }
    }
}
