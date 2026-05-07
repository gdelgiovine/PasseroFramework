using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Passero.Framework.Jwt
{
    /// <summary>
    /// Provides methods to create and decode JWT Bearer tokens.
    /// Supports both symmetric secret keys (HMAC-SHA256) and X.509 certificates (RSA-SHA256).
    /// </summary>
    public class JwtManager
    {
        private const string PayloadClaimType = "payload";

        private readonly string            _issuer;
        private readonly string            _audience;
        private readonly SigningCredentials _signingCredentials;
        private readonly SecurityKey       _validationKey;

        /// <summary>
        /// Initializes a new instance of <see cref="JwtManager"/> using a symmetric secret key (HMAC-SHA256).
        /// </summary>
        /// <param name="secretKey">The secret key used to sign tokens (at least 32 characters).</param>
        /// <param name="issuer">The token issuer.</param>
        /// <param name="audience">The token audience.</param>
        public JwtManager(string secretKey, string issuer = "", string audience = "")
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException(nameof(secretKey));

            if (Encoding.UTF8.GetByteCount(secretKey) < 32)
                throw new ArgumentException("La chiave simmetrica deve essere di almeno 32 caratteri (256 bit).", nameof(secretKey));

            _issuer   = issuer;
            _audience = audience;

            var symmetricKey    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            _signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            _validationKey      = symmetricKey;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JwtManager"/> using an X.509 certificate (RSA-SHA256).
        /// The certificate must include the private key for token creation.
        /// </summary>
        /// <param name="certificate">The X.509 certificate used to sign (and verify) tokens.</param>
        /// <param name="issuer">The token issuer.</param>
        /// <param name="audience">The token audience.</param>
        public JwtManager(X509Certificate2 certificate, string issuer = "", string audience = "")
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            _issuer   = issuer;
            _audience = audience;

            var x509SigningKey   = new X509SecurityKey(certificate);
            _signingCredentials  = new SigningCredentials(x509SigningKey, SecurityAlgorithms.RsaSha256);
            _validationKey       = x509SigningKey;
        }

        // ─── Token standard (claims) ────────────────────────────────────────────

        /// <summary>
        /// Creates a signed JWT Bearer token.
        /// </summary>
        /// <param name="claims">Optional collection of claims to embed in the token.</param>
        /// <param name="expireMinutes">Token lifetime in minutes (default: 60).</param>
        /// <returns>A compact-serialized JWT string.</returns>
        public string CreateToken(IEnumerable<Claim>? claims = null, int expireMinutes = 60)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject            = claims != null ? new ClaimsIdentity(claims) : new ClaimsIdentity(),
                Expires            = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = _signingCredentials
            };

            if (!string.IsNullOrEmpty(_issuer))
                tokenDescriptor.Issuer = _issuer;

            if (!string.IsNullOrEmpty(_audience))
                tokenDescriptor.Audience = _audience;

            var handler = new JwtSecurityTokenHandler();
            var token   = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Validates and decodes a JWT Bearer token.
        /// </summary>
        /// <param name="token">The compact-serialized JWT string.</param>
        /// <param name="principal">When successful, contains the <see cref="ClaimsPrincipal"/> extracted from the token.</param>
        /// <returns><c>true</c> if the token is valid; otherwise <c>false</c>.</returns>
        public bool TryDecodeToken(string token, out ClaimsPrincipal? principal)
        {
            principal = null;
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = _validationKey,
                    ValidateIssuer           = !string.IsNullOrEmpty(_issuer),
                    ValidIssuer              = _issuer,
                    ValidateAudience         = !string.IsNullOrEmpty(_audience),
                    ValidAudience            = _audience,
                    ValidateLifetime         = true,
                    ClockSkew                = TimeSpan.Zero
                };

                var handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Decodes a JWT token without validating its signature or expiry.
        /// Useful for inspecting the payload of an untrusted token.
        /// </summary>
        /// <param name="token">The compact-serialized JWT string.</param>
        /// <returns>A <see cref="JwtSecurityToken"/> instance, or <c>null</c> if parsing fails.</returns>
        public JwtSecurityToken? DecodeTokenUnsafe(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(token);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the claims contained in a valid JWT token.
        /// </summary>
        /// <param name="token">The compact-serialized JWT string.</param>
        /// <returns>The collection of claims, or an empty list if the token is invalid.</returns>
        public IEnumerable<Claim> GetClaims(string token)
        {
            if (TryDecodeToken(token, out var principal) && principal != null)
                return principal.Claims;

            return Array.Empty<Claim>();
        }

        // ─── Token con payload applicativo ──────────────────────────────────────

        /// <summary>
        /// Creates a signed JWT Bearer token embedding an application payload object
        /// serialized as JSON in the <c>payload</c> claim.
        /// </summary>
        /// <typeparam name="T">Type of the payload object.</typeparam>
        /// <param name="payload">The application payload to embed in the token.</param>
        /// <param name="additionalClaims">Optional additional claims to include alongside the payload.</param>
        /// <param name="expireMinutes">Token lifetime in minutes (default: 60).</param>
        /// <returns>A compact-serialized JWT string.</returns>
        public string CreatePayloadToken<T>(T payload, IEnumerable<Claim>? additionalClaims = null, int expireMinutes = 60)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            var json         = JsonSerializer.Serialize(payload);
            var payloadClaim = new Claim(PayloadClaimType, json, JsonClaimValueTypes.Json);

            var claims = new List<Claim> { payloadClaim };
            if (additionalClaims != null)
                claims.AddRange(additionalClaims);

            return CreateToken(claims, expireMinutes);
        }

        /// <summary>
        /// Validates a JWT token and deserializes the application payload from the <c>payload</c> claim.
        /// </summary>
        /// <typeparam name="T">Type of the payload object to deserialize.</typeparam>
        /// <param name="token">The compact-serialized JWT string.</param>
        /// <param name="payload">
        ///     When this method returns <c>true</c>, contains the deserialized payload;
        ///     otherwise the default value of <typeparamref name="T"/>.
        /// </param>
        /// <returns><c>true</c> if the token is valid and the payload was deserialized successfully; otherwise <c>false</c>.</returns>
        public bool TryDecodePayloadToken<T>(string token, out T? payload)
        {
            payload = default;
            try
            {
                if (!TryDecodeToken(token, out var principal) || principal == null)
                    return false;

                var json = principal.FindFirst(PayloadClaimType)?.Value;
                if (string.IsNullOrEmpty(json))
                    return false;

                payload = JsonSerializer.Deserialize<T>(json);
                return payload != null;
            }
            catch
            {
                return false;
            }
        }

        // ─── Utilità ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Generates a cryptographically secure random secret key suitable for HMAC-SHA256 signing.
        /// </summary>
        /// <param name="length">Key length in bytes (minimum 32, default 64).</param>
        /// <returns>A Base64-encoded string representing the generated key.</returns>
        public static string GenerateSecretKey(int length = 64)
        {
            if (length < 32)
                throw new ArgumentOutOfRangeException(nameof(length), "La lunghezza minima della chiave è 32 byte (256 bit).");

            var keyBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(keyBytes);

            return Convert.ToBase64String(keyBytes);
        }
    }
}
