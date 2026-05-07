namespace Passero.Framework.HttpHelper
{
    public sealed class PreWisejCommandResult
    {
        public int StatusCode { get; set; } = 200;

        public string ContentType { get; set; } = "text/plain; charset=utf-8";

        public string Body { get; set; } = string.Empty;
    }
}