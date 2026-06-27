using Passero.Framework.HttpHelper;

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
            result = new PreWisejCommandResult
            {
                StatusCode = 200,
                ContentType = "text/plain; charset=utf-8",
                Body = "Handled"
            };

            return true;
        }
    }
}