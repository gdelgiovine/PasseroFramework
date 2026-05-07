namespace Passero.Framework.HttpHelper
{
    public sealed class PingPreWisejCommandHandler : IPreWisejCommandHandler
    {
        public PreWisejCommandDefinition Definition { get; } = new PreWisejCommandDefinition
        {
            Name = "PING",
            RequiresHeaders = true,
            RequiresQueryString = true,
            RequiresBody = true,
        };

        public bool TryHandle(
            PreWisejRequestData request,
            out PreWisejCommandResult result)
        {
            result = new PreWisejCommandResult
            {
                StatusCode = 200,
                ContentType = "text/plain; charset=utf-8",
                Body = "PONG",
            };

            return true;
        }
    }
}