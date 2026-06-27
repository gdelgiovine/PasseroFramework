using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Dapper;
using Passero.Framework;

namespace Passero.Framework.Controls
{
    public sealed class NavFilterSqlOptions
    {
        public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;
        public bool CaseInsensitiveText { get; set; } = true;
        public bool AllowRelativeDateTokens { get; set; } = true;
        public bool AllowTextRelationalOperators { get; set; } = false;
        public bool UseLikeOperator { get; set; } = false;
     
    }

    public sealed class NavFilterError
    {
        public string Code { get; set; }
        public string TechnicalMessage { get; set; }
        public string UserMessage { get; set; }
        public int Position { get; set; } = -1;
    }

    public sealed class NavFilterBuildResult
    {
        public bool Success => Errors.Count == 0 && !string.IsNullOrWhiteSpace(Sql);
        public string Sql { get; set; }
        public List<NavFilterError> Errors { get; } = new List<NavFilterError>();
    }

    public static class NavFilterSqlEngine
    {
        public static NavFilterBuildResult BuildColumnPredicate(
            string columnName,
            Type propertyType,
            string filterText,
            bool isCodeColumn,
            string parameterPrefix,
            DynamicParameters parameters,
            NavFilterSqlOptions options,
            ProviderFeatures providerFeatures)
        {
            // Se providerFeatures è null, usa le feature di default (SQL Server)
            if (providerFeatures == null)
                providerFeatures = new ProviderFeatures();

            var result = new NavFilterBuildResult();
            string text = (filterText ?? string.Empty).Trim();

            if (text.Length == 0)
                return result;

            var orParts = SplitTopLevel(text, '|', result.Errors, "NAVFILT001", "Errore sintassi OR");
            if (result.Errors.Count > 0)
                return result;

            var orSql = new List<string>();
            int tokenIndex = 0;

            foreach (string orPart in orParts)
            {
                var andParts = SplitTopLevel(orPart, '&', result.Errors, "NAVFILT002", "Errore sintassi AND");
                if (result.Errors.Count > 0)
                    return result;

                var andSql = new List<string>();
                foreach (string atom in andParts)
                {
                    tokenIndex++;
                    string atomSql = BuildAtom(
                        columnName,
                        propertyType,
                        atom.Trim(),
                        isCodeColumn,
                        $"{parameterPrefix}_{tokenIndex}",
                        parameters,
                        options,
                        providerFeatures,
                        result.Errors);

                    if (!string.IsNullOrWhiteSpace(atomSql))
                        andSql.Add(atomSql);
                }

                if (andSql.Count > 0)
                    orSql.Add(andSql.Count == 1 ? andSql[0] : $"({string.Join(" AND ", andSql)})");
            }

            if (orSql.Count > 0)
                result.Sql = orSql.Count == 1 ? orSql[0] : $"({string.Join(" OR ", orSql)})";

            return result;
        }



        private static string BuildAtom(
            string columnName,
            Type propertyType,
            string atom,
            bool isCodeColumn,
            string parameterNameSeed,
            DynamicParameters parameters,
            NavFilterSqlOptions options,
            ProviderFeatures providerFeatures,
            List<NavFilterError> errors)
        {
            if (string.IsNullOrWhiteSpace(atom))
            {
                AddError(errors, "NAVFILT003", "Termine vuoto nel filtro.", "Filtro non valido.");
                return string.Empty;
            }

            string col = EscapeSqlIdentifier(columnName, providerFeatures);
            Type targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (atom == "''" || atom.Equals("<empty>", StringComparison.OrdinalIgnoreCase))
                return $"({col} IS NULL OR LTRIM(RTRIM(CONVERT(NVARCHAR(MAX), {col}))) = '')";

            if (atom == "<>''" || atom.Equals("<notempty>", StringComparison.OrdinalIgnoreCase))
                return $"({col} IS NOT NULL AND LTRIM(RTRIM(CONVERT(NVARCHAR(MAX), {col}))) <> '')";

            int rangeIndex = atom.IndexOf("..", StringComparison.Ordinal);
            if (rangeIndex >= 0)
            {
                string left = atom.Substring(0, rangeIndex).Trim();
                string right = atom.Substring(rangeIndex + 2).Trim();

                if (left.Length == 0 && right.Length == 0)
                {
                    AddError(errors, "NAVFILT004", "Range '..' non valido.", "Intervallo non valido.");
                    return string.Empty;
                }

                var parts = new List<string>();
                if (left.Length > 0)
                {
                    string pLo = $"{providerFeatures.ParameterPrefix}{parameterNameSeed}_lo";
                    object v = ConvertValue(left, targetType, isCodeColumn, options, errors);
                    if (errors.Count > 0)
                        return string.Empty;

                    if (v != null)
                    {
                        parameters.Add(pLo, v, Passero.Framework.Utilities.GetDbType(targetType));
                        parts.Add($"{col} >= {pLo}");
                    }
                }

                if (right.Length > 0)
                {
                    string pHi = $"{providerFeatures.ParameterPrefix}{parameterNameSeed}_hi";
                    object v = ConvertValue(right, targetType, isCodeColumn, options, errors);
                    if (errors.Count > 0)
                        return string.Empty;

                    if (v != null)
                    {
                        parameters.Add(pHi, v, Passero.Framework.Utilities.GetDbType(targetType));
                        parts.Add($"{col} <= {pHi}");
                    }
                }

                return parts.Count == 1 ? parts[0] : $"({string.Join(" AND ", parts)})";
            }

            string op = "=";
            string rawValue = atom;

            if (atom.StartsWith("<>", StringComparison.Ordinal)) { op = "<>"; rawValue = atom.Substring(2).Trim(); }
            else if (atom.StartsWith(">=", StringComparison.Ordinal)) { op = ">="; rawValue = atom.Substring(2).Trim(); }
            else if (atom.StartsWith("<=", StringComparison.Ordinal)) { op = "<="; rawValue = atom.Substring(2).Trim(); }
            else if (atom.StartsWith(">", StringComparison.Ordinal)) { op = ">"; rawValue = atom.Substring(1).Trim(); }
            else if (atom.StartsWith("<", StringComparison.Ordinal)) { op = "<"; rawValue = atom.Substring(1).Trim(); }
            else if (atom.StartsWith("=", StringComparison.Ordinal)) { op = "="; rawValue = atom.Substring(1).Trim(); }

            if (rawValue.Length == 0)
            {
                AddError(errors, "NAVFILT005", $"Valore mancante per operatore '{op}'.", "Valore filtro mancante.");
                return string.Empty;
            }

            // Se UseLikeOperator è true, usa sempre LIKE su campi testuali
            if (options.UseLikeOperator && targetType == typeof(string))
            {
                
                string p = $"{providerFeatures.ParameterPrefix}{parameterNameSeed}";

                string likeValue = "%" + rawValue + "%";
                parameters.Add(p, likeValue, DbType.String);

                string leftExpr = options.CaseInsensitiveText ? $"UPPER({col})" : col;
                string rightExpr = options.CaseInsensitiveText ? $"UPPER({p})" : p;

                if (op == "<>")
                    return $"{leftExpr} NOT LIKE {rightExpr}";

                return $"{leftExpr} LIKE {rightExpr}";
            }

            bool hasWildcard = rawValue.Contains("*") || rawValue.Contains("?");

            if (hasWildcard)
            {
                if (targetType != typeof(string))
                {
                    AddError(errors, "NAVFILT006", "Wildcard ammessi solo su campi testuali.", "Wildcard non supportata.");
                    return string.Empty;
                }

                
                string p = $"{providerFeatures.ParameterPrefix}{parameterNameSeed}";

                string likeValue = ToSqlLike(rawValue, isCodeColumn);
                parameters.Add(p, likeValue, DbType.String);

                string leftExpr = options.CaseInsensitiveText ? $"UPPER({col})" : col;
                string rightExpr = options.CaseInsensitiveText ? $"UPPER({p})" : p;

                if (op == "<>")
                    return $"{leftExpr} NOT LIKE {rightExpr} ESCAPE '\\'";

                return $"{leftExpr} LIKE {rightExpr} ESCAPE '\\'";
            }

            if (targetType == typeof(string) && !options.AllowTextRelationalOperators &&
                (op == ">" || op == "<" || op == ">=" || op == "<="))
            {
                AddError(errors, "NAVFILT007", "Confronto relazionale su testo non consentito.",
                    "Operatore non consentito per campo testuale.");
                return string.Empty;
            }

            
            string param = $"{providerFeatures.ParameterPrefix}{parameterNameSeed}";

            object value = ConvertValue(rawValue, targetType, isCodeColumn, options, errors);
            if (errors.Count > 0)
                return string.Empty;

            if (value != null)
            {
                parameters.Add(param, value, Passero.Framework.Utilities.GetDbType(targetType));
            }

            if (targetType == typeof(string) && options.CaseInsensitiveText)
                return $"UPPER({col}) {op} UPPER({param})";

            return $"{col} {op} {param}";
        }

        private static object ConvertValue(
            string raw,
            Type targetType,
            bool isCodeColumn,
            NavFilterSqlOptions options,
            List<NavFilterError> errors)
        {
            try
            {
                if (targetType == typeof(string))
                    return isCodeColumn ? raw.ToUpperInvariant() : raw;

                if (targetType == typeof(bool))
                {
                    string v = raw.Trim().ToLowerInvariant();
                    if (v == "true" || v == "yes" || v == "1") return true;
                    if (v == "false" || v == "no" || v == "0") return false;
                    throw new FormatException("Boolean non valido.");
                }

                if (targetType == typeof(Guid))
                {
                    if (Guid.TryParse(raw, out Guid g))
                        return g;
                    throw new FormatException("Guid non valido.");
                }

                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, raw, ignoreCase: true);
                }

                if (targetType == typeof(DateTime))
                    return ParseDateToken(raw, options);

                return Convert.ChangeType(raw, targetType, options.Culture);
            }
            catch (Exception ex)
            {
                AddError(errors, "NAVFILT008", ex.Message, $"Valore '{raw}' non valido per il tipo '{targetType.Name}'.");
                return null;
            }
        }

        private static DateTime ParseDateToken(string token, NavFilterSqlOptions options)
        {
            string t = token.Trim().ToUpperInvariant();

            if (options.AllowRelativeDateTokens)
            {
                if (t == "T" || t == "TODAY" || t == "W" || t == "WORKDATE")
                    return DateTime.Today;
                if (t == "CM")
                    return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                if (t == "CY")
                    return new DateTime(DateTime.Today.Year, 1, 1);
                if (t == "CW")
                {
                    int delta = DayOfWeek.Monday - DateTime.Today.DayOfWeek;
                    return DateTime.Today.AddDays(delta);
                }
            }

            return DateTime.Parse(token, options.Culture);
        }

        private static List<string> SplitTopLevel(
            string input,
            char separator,
            List<NavFilterError> errors,
            string code,
            string userMessage)
        {
            var parts = new List<string>();
            var current = new StringBuilder();

            foreach (char c in input)
            {
                if (c == separator)
                {
                    string p = current.ToString().Trim();
                    if (p.Length == 0)
                    {
                        AddError(errors, code, $"Separatore '{separator}' duplicato o termine mancante.", userMessage);
                        return parts;
                    }

                    parts.Add(p);
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            string last = current.ToString().Trim();
            if (last.Length == 0)
            {
                AddError(errors, code, $"Separatore '{separator}' finale non valido.", userMessage);
                return parts;
            }

            parts.Add(last);
            return parts;
        }

        private static string EscapeSqlIdentifier(string name, ProviderFeatures providerFeatures)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            string identifier = name.Trim();

            if (providerFeatures != null &&
                (providerFeatures.Dialect == DbDialect.DB2 || providerFeatures.Dialect == DbDialect.DB2i))
            {
                return identifier.ToUpperInvariant();
            }

            return providerFeatures.QuoteIdentifier(identifier);
        }

        private static string ToSqlLike(string navPattern, bool uppercase)
        {
            string s = navPattern
                .Replace("\\", "\\\\")
                .Replace("[", "\\[")
                .Replace("%", "\\%")
                .Replace("_", "\\_")
                .Replace("*", "%")
                .Replace("?", "_");

            return uppercase ? s.ToUpperInvariant() : s;
        }

        private static void AddError(List<NavFilterError> errors, string code, string technical, string user)
        {
            errors.Add(new NavFilterError
            {
                Code = code,
                TechnicalMessage = technical,
                UserMessage = user
            });
        }
    }
}