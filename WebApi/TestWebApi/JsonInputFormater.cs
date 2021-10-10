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
    public class JsonInputFormater : IInputFormatter, IApiRequestFormatMetadataProvider
    {
        public JsonInputFormater()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(CONTENT_TYPE));
        }

        public MediaTypeCollection SupportedMediaTypes { get; } = new MediaTypeCollection();

        protected const string CONTENT_TYPE = "application/json";
        public bool CanRead(InputFormatterContext context)
        {
            if (context.HttpContext.Request.ContentType == null)
            {
                return false;
            }

            var types = context.HttpContext.Request.ContentType.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            return types.Contains(CONTENT_TYPE);
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;
            if (context.HttpContext.Items.ContainsKey("EnableBodyBuffer"))
                request.Body.Position = 0;

            var key = "BodyString";
            string bodyString;
            if (context.HttpContext.Items.ContainsKey(key))
            {
                bodyString = context.HttpContext.Items[key].ToString();

            }
            else
            {
                using (var reader = context.ReaderFactory(request.Body, Encoding.UTF8))
                {
                    bodyString = reader.ReadToEnd();
                    context.HttpContext.Items[key] = bodyString;
                }
            }

            var result = JsonSerializer.DeserializeFromString(bodyString, context.ModelType);
            return await InputFormatterResult.SuccessAsync(result);
        }

        public IReadOnlyList<string> GetSupportedContentTypes(string contentType, Type objectType)
        {
            if (contentType == null)
            {
                return SupportedMediaTypes;
            }
            else
            {
                var parsedContentType = new MediaType(contentType);
                List<string> mediaTypes = null;
                foreach (var mediaType in SupportedMediaTypes)
                {
                    var parsedMediaType = new MediaType(mediaType);
                    if (parsedMediaType.IsSubsetOf(parsedContentType))
                    {
                        if (mediaTypes == null)
                        {
                            mediaTypes = new List<string>();
                        }

                        mediaTypes.Add(mediaType);
                    }
                }

                return mediaTypes;
            }
        }
    }
}
