using Passero.Framework.HttpHelper;
using Passero.Framework.JwtAuthHandler;

namespace PasseroDemo.Application
{
    public sealed class MyPreWisejCommandHandler : IPreWisejCommandHandler
    {
        public PreWisejCommandDefinition Definition { get; } = new PreWisejCommandDefinition
        {
            Name = "MYCMD",
            RequiresHeaders = true,
            RequiresQueryString = false,
            RequiresBody = false
        };

        public bool TryHandle(PreWisejRequestData request, out PreWisejCommandResult result)
        {
            //"0lQF1JSooP9m5bOFbycHmd6hyt7hP83xz7le0seAZEM=",
            //    issuer: "myApp",
            //    audience: "myApi");


            var options = new JwtAuthenticationOptions
            {
                SecretKey = "0lQF1JSooP9m5bOFbycHmd6hyt7hP83xz7le0seAZEM=",
                Issuer = "myApp",
                Audience = "myApi"
            };


            if (!JwtAuthHandler.TryAuthenticate(request.Path, request.Headers, options,true))
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