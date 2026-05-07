using System.Collections.Specialized;

namespace Passero.Framework.HttpHelper
{
    public sealed class PreWisejRequestData
    {
        public string Path { get; set; } = string.Empty;

        public string Method { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public NameValueCollection Headers { get; set; } = new NameValueCollection();

        public NameValueCollection QueryString { get; set; } = new NameValueCollection();
    }
}