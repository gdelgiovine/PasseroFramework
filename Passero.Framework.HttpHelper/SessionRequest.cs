using System.Collections.Specialized;
namespace Passero.Framework.HttpHelper
{
    /// <summary>
    /// Snapshot della richiesta HTTP iniziale della sessione utente.
    /// </summary>
    public sealed class SessionRequest
    {
        public string RawUrl { get; set; } = string.Empty;
        public string Scheme { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string UserHostAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public NameValueCollection Headers { get; set; } = new NameValueCollection();
        public NameValueCollection QueryParams { get; set; } = new NameValueCollection();
        public NameValueCollection Cookies { get; set; } = new NameValueCollection();

        public string GetHeader(string name) => Headers[name];
        public string GetQuery(string name) => QueryParams[name];
        public string GetCookie(string name) => Cookies[name];
    }
}