using System.Security.Cryptography.X509Certificates;

namespace Passero.Framework.JwtAuthHandler
{
    public sealed class JwtAuthenticationOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public X509Certificate2 Certificate { get; set; }
    }
}