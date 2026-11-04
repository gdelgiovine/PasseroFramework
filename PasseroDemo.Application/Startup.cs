using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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

            var microsoftConfigured = IsProviderConfigured(builder.Configuration, "Microsoft");
            var googleConfigured = IsProviderConfigured(builder.Configuration, "Google");
            var oidcConfigured = microsoftConfigured || googleConfigured;
            if (oidcConfigured)
            {
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });

                if (microsoftConfigured)
                {
                    builder.Services.AddAuthentication().AddOpenIdConnect("Microsoft", options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = builder.Configuration["Authentication:Microsoft:Authority"] ?? "https://login.microsoftonline.com/common/v2.0";
                        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
                        options.CallbackPath = builder.Configuration["Authentication:Microsoft:CallbackPath"] ?? "/signin-microsoft";
                        options.ResponseType = "code";
                        options.SaveTokens = false;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.TokenValidationParameters.NameClaimType = "name";
                        var allowedTenants = (builder.Configuration["Authentication:Microsoft:AllowedTenantIds"] ?? string.Empty)
                            .Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (allowedTenants.Length > 0)
                        {
                            options.TokenValidationParameters.IssuerValidator = (issuer, token, parameters) =>
                            {
                                var tenantId = (token as System.IdentityModel.Tokens.Jwt.JwtSecurityToken)?.Claims
                                    .FirstOrDefault(c => c.Type == "tid")?.Value;
                                if (tenantId != null && allowedTenants.Contains(tenantId, StringComparer.OrdinalIgnoreCase))
                                    return issuer;
                                throw new Microsoft.IdentityModel.Tokens.SecurityTokenInvalidIssuerException("Tenant non autorizzato.");
                            };
                        }
                    });
                }

                if (googleConfigured)
                {
                    builder.Services.AddAuthentication().AddOpenIdConnect("Google", options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = "https://accounts.google.com";
                        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                        options.CallbackPath = builder.Configuration["Authentication:Google:CallbackPath"] ?? "/signin-google";
                        options.ResponseType = "code";
                        options.SaveTokens = false;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.TokenValidationParameters.NameClaimType = "name";
                    });
                }
            }


            bool usePreWisejCommandRouter = true; // Set to true to enable the PreWisejCommandRouter middleware    

            if (usePreWisejCommandRouter)
            {
                // If using the PreWisejCommandRouter, we need to add the HttpContextAccessor   
                builder.Services.AddHttpContextAccessor();
                //Passero.Framework.HttpHelper.HttpContextHelper.CaptureToSession();
            }

            if (oidcConfigured)
                builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (oidcConfigured)
            {
                var contextAccessor = app.Services.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                Passero.Framework.Base.AuthenticatedSecuritySession.CurrentIdentityResolver = () =>
                {
                    var principal = contextAccessor.HttpContext?.User;
                    if (principal?.Identity?.IsAuthenticated != true)
                        return null;

                    var useImmutableId = string.Equals(builder.Configuration["Authentication:UseImmutableIdClaim"], "true", StringComparison.OrdinalIgnoreCase);
                    var userName = useImmutableId
                        ? (principal.FindFirst("oid")?.Value
                            ?? principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                            ?? principal.FindFirst("sub")?.Value
                            ?? principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                        : (principal.Identity.Name ?? principal.FindFirst("sub")?.Value);
                    return string.IsNullOrWhiteSpace(userName)
                        ? null
                        : new Passero.Framework.Base.AuthenticatedSecurityIdentity(userName, principal.Identity.AuthenticationType ?? "OIDC");
                };
            }

            if (oidcConfigured)
                app.UseAuthentication();

            JwtAuthHandler.AuthenticatedIdentityCallback = (userName, authenticationType) =>
                Passero.Framework.Base.AuthenticatedSecuritySession.SignIn(
                    new Passero.Framework.Base.AuthenticatedSecurityIdentity(userName, authenticationType));

            if (microsoftConfigured)
                app.MapGet("/auth/microsoft", () => Results.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" }, new[] { "Microsoft" }));
            if (googleConfigured)
                app.MapGet("/auth/google", () => Results.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" }, new[] { "Google" }));

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

             customHandlers.Add("MYCMD", new MyPreWisejCommandHandler(new JwtAuthenticationOptions
             {
                 SecretKey = builder.Configuration["Jwt:SecretKey"] ?? string.Empty,
                 Issuer = builder.Configuration["Jwt:Issuer"] ?? string.Empty,
                 Audience = builder.Configuration["Jwt:Audience"] ?? string.Empty
             }));


            if (usePreWisejCommandRouter)
            {
                // If using the PreWisejCommandRouter, we need to add the HttpContextAccessor   
                //builder.Services.AddHttpContextAccessor();
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

        private static bool IsProviderConfigured(IConfiguration configuration, string provider)
            => !string.IsNullOrWhiteSpace(configuration[$"Authentication:{provider}:ClientId"]) &&
               !string.IsNullOrWhiteSpace(configuration[$"Authentication:{provider}:ClientSecret"]);
    }
}
