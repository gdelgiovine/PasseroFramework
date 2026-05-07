#if !NET
using System;
using System.Collections.Generic;
using System.Web;
using Passero.Framework.HttpHelper;
using Passero.Framework.JwtAuthHandler;

namespace PasseroDemo.Application
{
    public class Global : HttpApplication
    {
        bool usePreWisejCommandRouter = false; // Set to true to enable the PreWisejCommandRouter    
        private static readonly PreWisejCommandRouter _router =
            CreateRouter();

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!usePreWisejCommandRouter)
                return; 

            var context = HttpContext.Current;
            if (context == null)
                return;

            var path = context.Request?.Url?.AbsolutePath ?? string.Empty;

            if (!_router.TryGetDefinition(path, out var definition))
                return;

            var requestData = PreWisejRequestAdapter.FromAspNetFramework(context, definition);

            if (_router.TryHandle(requestData, out var result))
            {
                context.Response.StatusCode = result.StatusCode;
                context.Response.ContentType = result.ContentType;
                context.Response.Write(result.Body);
                context.ApplicationInstance.CompleteRequest();
            }
        }

        private static PreWisejCommandRouter CreateRouter()
        {
            IDictionary<string, IPreWisejCommandHandler> customHandlers =
                new Dictionary<string, IPreWisejCommandHandler>(StringComparer.OrdinalIgnoreCase);

            // Example:
            // customHandlers.AddJwtPreWisejHandler(
            //     new JwtAuthenticationOptions
            //     {
            //         SecretKey = "", // System.Configuration.ConfigurationManager.AppSettings["Jwt.SecretKey"] ?? string.Empty,
            //         Issuer = "", // System.Configuration.ConfigurationManager.AppSettings["Jwt.Issuer"] ?? string.Empty,
            //         Audience = "", // System.Configuration.ConfigurationManager.AppSettings["Jwt.Audience"] ?? string.Empty,
            //     });

            return new PreWisejCommandRouter(customHandlers);
        }
    }
}
#endif