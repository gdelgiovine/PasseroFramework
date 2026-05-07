using System.Collections.Specialized;
using System.IO;
using System.Text;
#if NET
using Microsoft.AspNetCore.Http;
#endif

namespace Passero.Framework.HttpHelper
{
    public static class PreWisejRequestAdapter
    {
#if NET
        public static async System.Threading.Tasks.Task<PreWisejRequestData> FromAspNetCoreAsync(
            HttpContext context,
            PreWisejCommandDefinition definition)
        {
            var request = context.Request;
            var requestData = new PreWisejRequestData
            {
                Path = request.Path.Value ?? string.Empty,
                Method = request.Method,
            };

            if (definition.RequiresHeaders)
            {
                foreach (var header in request.Headers)
                    requestData.Headers[header.Key] = header.Value.ToString();
            }

            if (definition.RequiresQueryString)
            {
                foreach (var queryItem in request.Query)
                    requestData.QueryString[queryItem.Key] = queryItem.Value.ToString();
            }

            if (definition.RequiresBody)
            {
                request.EnableBuffering();

                if (request.Body.CanSeek)
                    request.Body.Position = 0;

                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestData.Body = await reader.ReadToEndAsync();
                }

                if (request.Body.CanSeek)
                    request.Body.Position = 0;
            }

            return requestData;
        }
#else
        public static PreWisejRequestData FromAspNetFramework(
            System.Web.HttpContext context,
            PreWisejCommandDefinition definition)
        {
            var request = context.Request;
            var requestData = new PreWisejRequestData
            {
                Path = request.Url?.AbsolutePath ?? string.Empty,
                Method = request.HttpMethod ?? string.Empty,
            };

            if (definition.RequiresHeaders)
                requestData.Headers = request.Headers;

            if (definition.RequiresQueryString)
            {
                foreach (string key in request.QueryString.AllKeys)
                {
                    if (key != null)
                        requestData.QueryString[key] = request.QueryString[key];
                }
            }

            if (definition.RequiresBody && request.InputStream != null && request.InputStream.CanSeek)
            {
                request.InputStream.Position = 0;

                using (var reader = new StreamReader(request.InputStream, Encoding.UTF8, true, 1024, true))
                {
                    requestData.Body = reader.ReadToEnd();
                }

                request.InputStream.Position = 0;
            }

            return requestData;
        }
#endif
    }
}