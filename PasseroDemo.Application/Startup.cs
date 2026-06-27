using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passero.Framework.HttpHelper;
using Passero.Framework.JwtAuthHandler;
using Wisej.Core;
using Wisej.Web;

namespace PasseroDemo.Application
{
    /// <summary>
    /// The Startup class configures services and the app's request pipeline.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                WebRootPath = "./"
            });


            bool usePreWisejCommandRouter = false; // Set to true to enable the PreWisejCommandRouter middleware    

            if (usePreWisejCommandRouter)
            {
                // If using the PreWisejCommandRouter, we need to add the HttpContextAccessor   
                builder.Services.AddHttpContextAccessor();
                Passero.Framework.HttpHelper.HttpContextHelper.CaptureToSession();
            }

            var app = builder.Build();

            // Dictionary to hold custom PreWisej command handlers, if any are needed.
            IDictionary<string, IPreWisejCommandHandler> customHandlers = new Dictionary<string, IPreWisejCommandHandler>(StringComparer.OrdinalIgnoreCase);


            // Example:
             customHandlers.AddJwtPreWisejHandler(
                 new JwtAuthenticationOptions
                 {
                     SecretKey = builder.Configuration["Jwt:SecretKey"] ?? string.Empty,
                     Issuer = builder.Configuration["Jwt:Issuer"] ?? string.Empty,
                     Audience = builder.Configuration["Jwt:Audience"] ?? string.Empty,
                 });

            customHandlers.Add("MYCMD", new MyPreWisejCommandHandler () );  


            if (usePreWisejCommandRouter)
            {
                // If using the PreWisejCommandRouter, we need to add the HttpContextAccessor   
                builder.Services.AddHttpContextAccessor();
                Passero.Framework.HttpHelper.HttpContextHelper.CaptureToSession();

                var router = new PreWisejCommandRouter(customHandlers);
                app.Use(async (context, next) =>
                {
                    var path = context.Request.Path.Value ?? string.Empty;
                    if (!router.TryGetDefinition(path, out var definition))
                    {
                        await next();
                        return;
                    }
                    var requestData = await PreWisejRequestAdapter.FromAspNetCoreAsync(context, definition);
                    if (router.TryHandle(requestData, out var result))
                    {
                        context.Response.StatusCode = result.StatusCode;
                        context.Response.ContentType = result.ContentType;
                        await context.Response.WriteAsync(result.Body);
                        return;
                    }
                    await next();
                });
            }

            

            app.UseWisej();
            app.UseFileServer();
            app.Run();
        }
    }
}
