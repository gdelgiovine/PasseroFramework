using System.Collections.Specialized;
using System.IO;
using System.Text;
using Wisej.Web;
#if NET
using Microsoft.AspNetCore.Http;
#endif

namespace Passero.Framework.HttpHelper
{
    /// <summary>
    /// Fornisce accesso alla request HTTP della sessione corrente,
    /// indipendentemente dal framework (.NET 8 / .NET Framework 4.8).
    /// </summary>
    public static class HttpContextHelper
    {
        private const string SessionKeyRequest = "_SessionRequest";

#if NET
        private static IHttpContextAccessor? _accessor;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
#endif

        /// <summary>
        /// Cattura la request HTTP corrente e la salva nella sessione Wisej.
        /// </summary>
        public static void CaptureToSession()
        {
            Wisej.Web.Application.Session[SessionKeyRequest] = BuildSessionRequest();
        }

        /// <summary>
        /// Restituisce la request della sessione utente corrente.
        /// </summary>
        public static SessionRequest Current
            => Wisej.Web.Application.Session[SessionKeyRequest] as SessionRequest;

        private static SessionRequest BuildSessionRequest()
        {
#if NET
            return BuildSessionRequest(_accessor?.HttpContext);
#else
            return BuildSessionRequestNet48();
#endif
        }

#if NET
        private static SessionRequest BuildSessionRequest(HttpContext? context)
        {
            var req = context?.Request;
            if (req == null)
                return new SessionRequest();

            var headers = new NameValueCollection();
            var queryParams = new NameValueCollection();
            var cookies = new NameValueCollection();

            foreach (var h in req.Headers)
                headers[h.Key] = h.Value.ToString();

            foreach (var q in req.Query)
                queryParams[q.Key] = q.Value.ToString();

            foreach (var c in req.Cookies)
                cookies[c.Key] = c.Value;

            var body = string.Empty;

            req.EnableBuffering();

            if (req.Body.CanSeek)
                req.Body.Position = 0;

            using (var reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                body = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            if (req.Body.CanSeek)
                req.Body.Position = 0;

            return new SessionRequest
            {
                RawUrl = (req.Path.Value + req.QueryString.Value) ?? string.Empty,
                Scheme = req.Scheme,
                Host = req.Host.ToString(),
                Path = req.Path.Value ?? string.Empty,
                QueryString = req.QueryString.Value ?? string.Empty,
                Method = req.Method,
                UserHostAddress = context?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                UserAgent = req.Headers["User-Agent"].ToString(),
                ContentType = req.ContentType ?? string.Empty,
                Body = body,
                Headers = headers,
                QueryParams = queryParams,
                Cookies = cookies,
            };
        }
#else
        private static SessionRequest BuildSessionRequestNet48()
        {
            var req = System.Web.HttpContext.Current.Request;
            if (req == null)
                return new SessionRequest();

            var cookies = new NameValueCollection();
            for (int i = 0; i < req.Cookies.Count; i++)
            {
                var cookie = req.Cookies[i];
                if (cookie != null)
                    cookies[cookie.Name] = cookie.Value;
            }

            var body = string.Empty;
            if (req.InputStream != null && req.InputStream.CanSeek)
            {
                req.InputStream.Position = 0;
                using (var reader = new StreamReader(req.InputStream, Encoding.UTF8, true, 1024, true))
                {
                    body = reader.ReadToEnd();
                }
                req.InputStream.Position = 0;
            }

            return new SessionRequest
            {
                RawUrl = req.RawUrl ?? string.Empty,
                Scheme = req.Url?.Scheme ?? string.Empty,
                Host = req.Url?.Host ?? string.Empty,
                Path = req.Path ?? string.Empty,
                QueryString = req.Url?.Query ?? string.Empty,
                Method = req.HttpMethod ?? string.Empty,
                UserHostAddress = req.UserHostAddress ?? string.Empty,
                UserAgent = req.UserAgent ?? string.Empty,
                ContentType = req.ContentType ?? string.Empty,
                Body = body,
                Headers = req.Headers,
                QueryParams = req.QueryString,
                Cookies = cookies,
            };
        }
#endif
    }
}