namespace Passero.Framework.JwtAuthHandler
{
    public sealed class JwtAuthenticationResult
    {
        public string AuthToken { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string PayloadJson { get; set; } = string.Empty;
    }
}