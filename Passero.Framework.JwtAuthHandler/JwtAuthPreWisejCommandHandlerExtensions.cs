using System;
using System.Collections.Generic;
using Passero.Framework.HttpHelper;

namespace Passero.Framework.JwtAuthHandler
{
    public static class JwtAuthPreWisejCommandHandlerExtensions
    {
        public static IDictionary<string, IPreWisejCommandHandler> AddJwtPreWisejHandler(
            this IDictionary<string, IPreWisejCommandHandler> handlers,
            JwtAuthenticationOptions options)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            var handler = new JwtAuthPreWisejCommandHandler(options ?? new JwtAuthenticationOptions());
            handlers[handler.Definition.Name] = handler;

            return handlers;
        }

        public static IDictionary<string, IPreWisejCommandHandler> AddJwtPreWisejHandler(
            this IDictionary<string, IPreWisejCommandHandler> handlers,
            string secretKey,
            string issuer = "",
            string audience = "")
        {
            return handlers.AddJwtPreWisejHandler(new JwtAuthenticationOptions
            {
                SecretKey = secretKey,
                Issuer = issuer,
                Audience = audience,
            });
        }
    }
}
