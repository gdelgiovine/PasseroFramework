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
        /// <summary>Chiave di sessione per il token JWT grezzo di autenticazione.</summary>
        public const string SessionKeyAuthToken = "JWT_AUTH_TOKEN";

        /// <summary>Chiave di sessione per il payload JSON grezzo estratto da X-Payload-Token.</summary>
        public const string SessionKeyPayload = "JWT_PAYLOAD";

        /// <summary>Chiave di sessione per il nome utente estratto dal token di autenticazione.</summary>
        public const string SessionKeyJwtUser = "JWT_USER";

        /// <summary>Segmento URL che attiva il flusso di autenticazione JWT.</summary>
        public const string JwtAuthUrlSegment = "/JWTAUTH";

        public static bool TryValidateRequest(
            string requestUrl,
            NameValueCollection headers,
            JwtAuthenticationOptions options,
            out JwtAuthenticationResult authenticationResult)
        {
            authenticationResult = null;

            if (options == null)
                return false;

            if (string.IsNullOrEmpty(requestUrl) || headers == null)
                return false;

            if (requestUrl.IndexOf(JwtAuthUrlSegment, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            var authHeader = headers["Authorization"] ?? string.Empty;
            if (authHeader.IndexOf("Bearer ", StringComparison.OrdinalIgnoreCase) != 0)
                return false;

            var authToken = authHeader.Substring("Bearer ".Length).Trim();
            var jwtManager = new JwtManager(
                options.SecretKey,
                options.Issuer,
                options.Audience);

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
            JwtAuthenticationOptions options)
        {
            if (!TryValidateRequest(requestUrl, headers, options, out var authenticationResult))
                return false;

            Application.Session[SessionKeyAuthToken] = authenticationResult.AuthToken;
            Application.Session[SessionKeyJwtUser] = authenticationResult.UserName;

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
