using System;
using System.Collections.Specialized;
using Passero.Framework.Jwt;
using Wisej.Web;

namespace Passero.Framework.JwtAuthHandler
{
    /// <summary>
    /// Gestisce l'autenticazione JWT per le richieste Wisej.NET che contengono /JWTAUTH nell'URL.
    /// Legge l'header <c>Authorization: Bearer</c> (token di autenticazione) e,
    /// opzionalmente, l'header <c>X-Payload-Token</c> (token con payload applicativo).
    /// </summary>
    public static class JwtAuthHandler
    {
        public static Action<string, string> AuthenticatedIdentityCallback { get; set; }

        /// <summary>Chiave di sessione per il token JWT grezzo di autenticazione.</summary>
        public const string SessionKeyAuthToken = "JWT_AUTH_TOKEN";

        /// <summary>Chiave di sessione per il payload JSON grezzo estratto da X-Payload-Token.</summary>
        public const string SessionKeyPayload = "JWT_PAYLOAD";

        /// <summary>Chiave di sessione per il nome utente estratto dal token di autenticazione.</summary>
        public const string SessionKeyJwtUser = "JWT_USER";

        /// <summary>Segmento URL che attiva il flusso di autenticazione JWT.</summary>
        //public const string JwtAuthUrlSegment = "/JWTAUTH";
        public static string JwtAuthUrlSegment { get; set; } = "/JWTAUTH";
        public static bool TryValidateRequest(
            string requestUrl,
            NameValueCollection headers,
            JwtAuthenticationOptions options,
            out JwtAuthenticationResult authenticationResult,
            bool isPreWisejCommand = false )
        {
            authenticationResult = null;

            if (options == null)
                return false;

            if ((string.IsNullOrWhiteSpace(options.SecretKey) && options.Certificate == null) ||
                string.IsNullOrWhiteSpace(options.Issuer) ||
                string.IsNullOrWhiteSpace(options.Audience))
                return false;

            if (string.IsNullOrEmpty(requestUrl) || headers == null)
                return false;


            if (!isPreWisejCommand && requestUrl.IndexOf(JwtAuthUrlSegment, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            var authHeader = headers["Authorization"] ?? string.Empty;
            if (authHeader.IndexOf("Bearer ", StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            var authToken = authHeader.Substring("Bearer ".Length).Trim();
            var jwtManager = options.Certificate != null
                ? new JwtManager(options.Certificate, options.Issuer, options.Audience)
                : new JwtManager(options.SecretKey, options.Issuer, options.Audience);

            if (!jwtManager.TryDecodeToken(authToken, out var principal) || principal == null)
                return false;

            var userName = principal.Identity?.Name
                ?? principal.FindFirst("sub")?.Value
                ?? string.Empty;

            var result = new JwtAuthenticationResult
            {
                AuthToken = authToken,
                UserName = userName,
            };

            var payloadHeader = headers["X-Payload-Token"] ?? string.Empty;
            if (!string.IsNullOrEmpty(payloadHeader))
            {
                if (jwtManager.TryDecodeToken(payloadHeader, out var payloadPrincipal) && payloadPrincipal != null)
                    result.PayloadJson = payloadPrincipal.FindFirst("payload")?.Value ?? string.Empty;
            }

            authenticationResult = result;
            return true;
        }

        public static bool TryAuthenticate(
            string requestUrl,
            NameValueCollection headers,
            JwtAuthenticationOptions options,
            bool isPreWisejCommand =false)
        {
            if (!TryValidateRequest(requestUrl, headers, options, out var authenticationResult,isPreWisejCommand))
                return false;

            Application.Session[SessionKeyAuthToken] = authenticationResult.AuthToken;
            Application.Session[SessionKeyJwtUser] = authenticationResult.UserName;
            if (!string.IsNullOrWhiteSpace(authenticationResult.UserName))
                AuthenticatedIdentityCallback?.Invoke(authenticationResult.UserName, "JWT");

            if (!string.IsNullOrEmpty(authenticationResult.PayloadJson))
                Application.Session[SessionKeyPayload] = authenticationResult.PayloadJson;

            return true;
        }

        // Overload legacy per retrocompatibilità.
        public static bool TryValidateRequest(
            string requestUrl,
            NameValueCollection headers,
            string secretKey,
            out JwtAuthenticationResult authenticationResult,
            string issuer = "",
            string audience = "")
        {
            var options = new JwtAuthenticationOptions
            {
                SecretKey = secretKey,
                Issuer = issuer,
                Audience = audience,
            };

            return TryValidateRequest(requestUrl, headers, options, out authenticationResult);
        }

        // Overload legacy per retrocompatibilità.
        public static bool TryAuthenticate(
            string requestUrl,
            NameValueCollection headers,
            string secretKey,
            string issuer = "",
            string audience = "")
        {
            var options = new JwtAuthenticationOptions
            {
                SecretKey = secretKey,
                Issuer = issuer,
                Audience = audience,
            };

            return TryAuthenticate(requestUrl, headers, options);
        }

        /// <summary>
        /// Recupera dalla sessione corrente il nome utente autenticato tramite JWT.
        /// </summary>
        public static string? GetSessionUser()
            => Application.Session[SessionKeyJwtUser] as string;

        /// <summary>
        /// Recupera dalla sessione corrente il payload JSON grezzo.
        /// </summary>
        public static string? GetSessionPayloadJson()
            => Application.Session[SessionKeyPayload] as string;

        /// <summary>
        /// Recupera e deserializza dalla sessione il payload applicativo nel tipo specificato.
        /// </summary>
        public static T? GetSessionPayload<T>()
        {
            var json = GetSessionPayloadJson();
            if (string.IsNullOrEmpty(json))
                return default;

            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }
    }
}
