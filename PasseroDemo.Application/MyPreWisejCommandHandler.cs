using Passero.Framework.HttpHelper;
using Passero.Framework.JwtAuthHandler;

namespace PasseroDemo.Application
{
    public sealed class MyPreWisejCommandHandler : IPreWisejCommandHandler
    {
        private readonly JwtAuthenticationOptions _options;

        public MyPreWisejCommandHandler(JwtAuthenticationOptions options)
        {
            _options = options ?? new JwtAuthenticationOptions();
            JwtAuthHandler.AuthenticatedIdentityCallback = (userName, authenticationType) =>
                Passero.Framework.Base.AuthenticatedSecuritySession.SignIn(
                    new Passero.Framework.Base.AuthenticatedSecurityIdentity(userName, authenticationType));
        }

        public PreWisejCommandDefinition Definition { get; } = new PreWisejCommandDefinition
        {
            Name = "MYCMD",
            RequiresHeaders = true,
            RequiresQueryString = false,
            RequiresBody = false
        };

        public bool TryHandle(PreWisejRequestData request, out PreWisejCommandResult result)
        {
            if (!JwtAuthHandler.TryAuthenticate(request.Path, request.Headers, _options, true))
            {
                result = new PreWisejCommandResult
                {
                    StatusCode = 401,
                    ContentType = "text/plain; charset=utf-8",
                    Body = "Unauthorized"
                };

                return true;
            }

            result = new PreWisejCommandResult
            {
                StatusCode = 200,
                ContentType = "text/plain; charset=utf-8",
                Body = "OK"
            };

            return true;
        }
    }
}