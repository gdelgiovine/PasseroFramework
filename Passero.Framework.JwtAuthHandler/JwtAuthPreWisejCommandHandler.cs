using Passero.Framework.HttpHelper;

namespace Passero.Framework.JwtAuthHandler
{
    public sealed class JwtAuthPreWisejCommandHandler : IPreWisejCommandHandler
    {
        private readonly JwtAuthenticationOptions _options;

        public JwtAuthPreWisejCommandHandler(JwtAuthenticationOptions options)
        {
            _options = options ?? new JwtAuthenticationOptions();
        }

        public PreWisejCommandDefinition Definition { get; } = new PreWisejCommandDefinition
        {
            Name = "JWTAUTH",
            RequiresHeaders = true,
            RequiresQueryString = false,
            RequiresBody = false,
        };

        public bool TryHandle(
            PreWisejRequestData request,
            out PreWisejCommandResult result)
        {
            if (JwtAuthHandler.TryAuthenticate(request.Path, request.Headers, _options))
            {
                result = new PreWisejCommandResult
                {
                    StatusCode = 200,
                    ContentType = "text/plain; charset=utf-8",
                    Body = "OK",
                };

                return true;
            }

            result = new PreWisejCommandResult
            {
                StatusCode = 401,
                ContentType = "text/plain; charset=utf-8",
                Body = "Unauthorized",
            };

            return true;
        }
    }
}
