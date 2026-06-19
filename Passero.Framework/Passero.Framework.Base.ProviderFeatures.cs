
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Dapper;

namespace Passero.Framework
{


    public enum DbDialect
    {
        Unknown = 0,
        SqlServer = 1,
        SqlServerCe = 2,
        PostgreSql = 3,
        MySql = 4,
        MariaDb = 5,
        SQLite = 6,
        Oracle = 7,
        Firebird = 8,
        Odbc = 9,
        OleDb = 10
    }

    public enum IdentifierQuoteStyle
    {
        None = 0,
        SquareBrackets = 1, // [Table]
        DoubleQuotes = 2,   // "Table"
        Backticks = 3       // `Table`
    }

    public enum LimitStyle
    {
        None = 0,

        // SQL Server / SQL Server CE
        Top = 1,             // SELECT TOP (@Take) ...

        // MySQL / MariaDB / SQLite / PostgreSQL
        Limit = 2,           // LIMIT @Take
        LimitOffset = 3,     // LIMIT @Take OFFSET @Skip

        // SQL Server 2012+, Oracle 12c+, PostgreSQL also supports this syntax
        OffsetFetch = 4,     // OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY

        // Oracle / DB2 style
        FetchFirst = 5,      // FETCH FIRST @Take ROWS ONLY

        // Firebird
        Rows = 6,            // ROWS @From TO @To
        FirstSkip = 7,       // FIRST @Take SKIP @Skip

        // Oracle pre-12c
        RowNum = 8           // WHERE ROWNUM <= :Take
    }

    public enum IdentityRetrievalStyle
    {
        None = 0,
        ScopeIdentity = 1,       // SQL Server: SELECT CAST(SCOPE_IDENTITY() AS int)
        LastInsertId = 2,        // MySQL / MariaDB: SELECT LAST_INSERT_ID()
        LastInsertRowId = 3,     // SQLite: SELECT last_insert_rowid()
        Returning = 4,           // PostgreSQL / Oracle / Firebird: INSERT ... RETURNING Id
        OutputInserted = 5       // SQL Server: OUTPUT INSERTED.Id
    }

    public sealed class ProviderFeatures
    {
        public DbDialect Dialect { get; set; }
        public string ProviderName { get; set; }

        public string ParameterPrefix { get; set; }
        public bool SupportsNamedParameters { get; set; }
        public bool SupportsPositionalParameters { get; set; }

        public IdentifierQuoteStyle DefaultIdentifierQuote { get; set; }
        public bool SupportsSquareBracketIdentifiers { get; set; }
        public bool SupportsDoubleQuoteIdentifiers { get; set; }
        public bool SupportsBacktickIdentifiers { get; set; }

        public LimitStyle LimitStyle { get; set; }
        public bool SupportsOffsetPaging { get; set; }

        public bool SupportsReturningClause { get; set; }
        public bool SupportsOutputInserted { get; set; }
        public bool SupportsLastInsertIdFunction { get; set; }
        public IdentityRetrievalStyle IdentityRetrievalStyle { get; set; }

        public bool SupportsTransactions { get; set; }
        public bool SupportsMultipleResultSets { get; set; }
        public bool SupportsStoredProcedures { get; set; }

        public ProviderFeatures()
        {
            Dialect = DbDialect.Unknown;
            ProviderName = "Unknown";

            ParameterPrefix = "@";
            SupportsNamedParameters = true;
            SupportsPositionalParameters = false;

            DefaultIdentifierQuote = IdentifierQuoteStyle.None;
            SupportsSquareBracketIdentifiers = false;
            SupportsDoubleQuoteIdentifiers = false;
            SupportsBacktickIdentifiers = false;

            LimitStyle = LimitStyle.None;
            SupportsOffsetPaging = false;

            SupportsReturningClause = false;
            SupportsOutputInserted = false;
            SupportsLastInsertIdFunction = false;
            IdentityRetrievalStyle = IdentityRetrievalStyle.None;

            SupportsTransactions = true;
            SupportsMultipleResultSets = false;
            SupportsStoredProcedures = false;
        }

        public string QuoteIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier non valido.", "identifier");

            switch (DefaultIdentifierQuote)
            {
                case IdentifierQuoteStyle.SquareBrackets:
                    return "[" + identifier.Replace("]", "]]") + "]";

                case IdentifierQuoteStyle.DoubleQuotes:
                    return "\"" + identifier.Replace("\"", "\"\"") + "\"";

                case IdentifierQuoteStyle.Backticks:
                    return "`" + identifier.Replace("`", "``") + "`";

                default:
                    return identifier;
            }
        }

        public string QuoteQualifiedIdentifier(string qualifiedIdentifier)
        {
            if (string.IsNullOrWhiteSpace(qualifiedIdentifier))
                throw new ArgumentException("Identifier non valido.", "qualifiedIdentifier");

            string[] parts = qualifiedIdentifier.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = QuoteIdentifier(parts[i].Trim());
            }

            return string.Join(".", parts);
        }

        public string Parameter(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome parametro non valido.", "name");

            name = TrimParameterPrefix(name);

            if (SupportsPositionalParameters && !SupportsNamedParameters)
                return "?";

            return ParameterPrefix + name;
        }

        public string NormalizeParameterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome parametro non valido.", "name");

            return TrimParameterPrefix(name);
        }

        private static string TrimParameterPrefix(string name)
        {
            return name.TrimStart('@', ':', '?', '$');
        }
    }

    public static class ProviderFeaturesResolver
    {
        public static ProviderFeatures FromConnection(IDbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            Type type = connection.GetType();

            string providerTypeName =
                type.FullName ??
                type.Name ??
                "Unknown";

            return FromProviderTypeName(providerTypeName);
        }

        public static ProviderFeatures FromProviderTypeName(string providerTypeName)
        {
            if (string.IsNullOrWhiteSpace(providerTypeName))
                return Unknown("Unknown");

            string p = providerTypeName.ToLowerInvariant();

            if (ContainsAny(p,
                "microsoft.data.sqlclient",
                "system.data.sqlclient"))
            {
                return SqlServer(providerTypeName);
            }

            if (ContainsAny(p,
                "sqlserverce",
                "sqlce",
                "system.data.sqlserverce"))
            {
                return SqlServerCe(providerTypeName);
            }

            if (ContainsAny(p, "npgsql"))
            {
                return PostgreSql(providerTypeName);
            }

            if (ContainsAny(p,
                "mysqlconnector",
                "mysql.data",
                "mysqlclient"))
            {
                return MySql(providerTypeName);
            }

            if (ContainsAny(p, "mariadb"))
            {
                return MariaDb(providerTypeName);
            }

            if (ContainsAny(p,
                "microsoft.data.sqlite",
                "system.data.sqlite",
                "sqliteconnection",
                "sqlite"))
            {
                return SQLite(providerTypeName);
            }

            if (ContainsAny(p,
                "oracle.manageddataaccess",
                "oracle.dataaccess",
                "oracleconnection"))
            {
                return Oracle(providerTypeName);
            }

            if (ContainsAny(p,
                "firebirdsql",
                "fbconnection",
                "firebird"))
            {
                return Firebird(providerTypeName);
            }

            if (ContainsAny(p, "odbc"))
            {
                return Odbc(providerTypeName);
            }

            if (ContainsAny(p, "oledb"))
            {
                return OleDb(providerTypeName);
            }

            return Unknown(providerTypeName);
        }

        private static bool ContainsAny(string value, params string[] tokens)
        {
            if (value == null)
                return false;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (value.IndexOf(tokens[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private static ProviderFeatures SqlServer(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.SqlServer,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.SquareBrackets,
                SupportsSquareBracketIdentifiers = true,
                SupportsDoubleQuoteIdentifiers = true,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.Top,
                SupportsOffsetPaging = true,

                SupportsReturningClause = false,
                SupportsOutputInserted = true,
                SupportsLastInsertIdFunction = true,
                IdentityRetrievalStyle = IdentityRetrievalStyle.OutputInserted,

                SupportsTransactions = true,
                SupportsMultipleResultSets = true,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures SqlServerCe(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.SqlServerCe,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.SquareBrackets,
                SupportsSquareBracketIdentifiers = true,
                SupportsDoubleQuoteIdentifiers = false,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.Top,
                SupportsOffsetPaging = false,

                SupportsReturningClause = false,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = true,
                IdentityRetrievalStyle = IdentityRetrievalStyle.ScopeIdentity,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = false
            };
        }

        private static ProviderFeatures PostgreSql(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.PostgreSql,
                ProviderName = provider,

                // Con Dapper + Npgsql, @Nome è normalmente la scelta più pratica.
                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.DoubleQuotes,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = true,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.LimitOffset,
                SupportsOffsetPaging = true,

                SupportsReturningClause = true,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.Returning,

                SupportsTransactions = true,
                SupportsMultipleResultSets = true,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures MySql(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.MySql,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.Backticks,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = false,
                SupportsBacktickIdentifiers = true,

                LimitStyle = LimitStyle.LimitOffset,
                SupportsOffsetPaging = true,

                SupportsReturningClause = false,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = true,
                IdentityRetrievalStyle = IdentityRetrievalStyle.LastInsertId,

                SupportsTransactions = true,
                SupportsMultipleResultSets = true,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures MariaDb(string provider)
        {
            ProviderFeatures features = MySql(provider);
            features.Dialect = DbDialect.MariaDb;
            features.ProviderName = provider;
            return features;
        }

        private static ProviderFeatures SQLite(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.SQLite,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = true,

                DefaultIdentifierQuote = IdentifierQuoteStyle.DoubleQuotes,
                SupportsSquareBracketIdentifiers = true,
                SupportsDoubleQuoteIdentifiers = true,
                SupportsBacktickIdentifiers = true,

                LimitStyle = LimitStyle.LimitOffset,
                SupportsOffsetPaging = true,

                SupportsReturningClause = true,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = true,
                IdentityRetrievalStyle = IdentityRetrievalStyle.LastInsertRowId,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = false
            };
        }

        private static ProviderFeatures Oracle(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.Oracle,
                ProviderName = provider,

                ParameterPrefix = ":",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.DoubleQuotes,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = true,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.OffsetFetch,
                SupportsOffsetPaging = true,

                SupportsReturningClause = true,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.Returning,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures Firebird(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.Firebird,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.DoubleQuotes,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = true,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.Rows,
                SupportsOffsetPaging = true,

                SupportsReturningClause = true,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.Returning,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures Odbc(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.Odbc,
                ProviderName = provider,

                ParameterPrefix = "?",
                SupportsNamedParameters = false,
                SupportsPositionalParameters = true,

                DefaultIdentifierQuote = IdentifierQuoteStyle.None,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = false,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.None,
                SupportsOffsetPaging = false,

                SupportsReturningClause = false,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.None,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures OleDb(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.OleDb,
                ProviderName = provider,

                ParameterPrefix = "?",
                SupportsNamedParameters = false,
                SupportsPositionalParameters = true,

                DefaultIdentifierQuote = IdentifierQuoteStyle.None,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = false,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.None,
                SupportsOffsetPaging = false,

                SupportsReturningClause = false,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.None,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = true
            };
        }

        private static ProviderFeatures Unknown(string provider)
        {
            return new ProviderFeatures
            {
                Dialect = DbDialect.Unknown,
                ProviderName = provider,

                ParameterPrefix = "@",
                SupportsNamedParameters = true,
                SupportsPositionalParameters = false,

                DefaultIdentifierQuote = IdentifierQuoteStyle.None,
                SupportsSquareBracketIdentifiers = false,
                SupportsDoubleQuoteIdentifiers = false,
                SupportsBacktickIdentifiers = false,

                LimitStyle = LimitStyle.None,
                SupportsOffsetPaging = false,

                SupportsReturningClause = false,
                SupportsOutputInserted = false,
                SupportsLastInsertIdFunction = false,
                IdentityRetrievalStyle = IdentityRetrievalStyle.None,

                SupportsTransactions = true,
                SupportsMultipleResultSets = false,
                SupportsStoredProcedures = false
            };
        }
    }

    public static class SqlDialectBuilder
    {
        public static string ApplyLimit(
            string baseSelectSql,
            ProviderFeatures features,
            int limitvalue)
        {
            if (string.IsNullOrWhiteSpace(baseSelectSql))
                throw new ArgumentException("SQL non valido.", "baseSelectSql");

            if (features == null)
                throw new ArgumentNullException("features");

            string _limitValue = limitvalue.ToString(); 

            switch (features.LimitStyle)
            {
                case LimitStyle.Top:
                    return InsertTop(baseSelectSql, _limitValue);

                case LimitStyle.Limit:
                case LimitStyle.LimitOffset:
                    return baseSelectSql + " LIMIT " + _limitValue;

                case LimitStyle.FetchFirst:
                    return baseSelectSql + " FETCH FIRST " + _limitValue + " ROWS ONLY";

                case LimitStyle.OffsetFetch:
                    return baseSelectSql + " OFFSET 0 ROWS FETCH NEXT " + _limitValue + " ROWS ONLY";

                case LimitStyle.RowNum:
                    return "SELECT * FROM (" + baseSelectSql + ") WHERE ROWNUM <= " + _limitValue;

                case LimitStyle.FirstSkip:
                    return InsertFirebirdFirstSkip(baseSelectSql, _limitValue, null);

                default:
                    throw new NotSupportedException(
                        "Limit non supportato per provider " + features.ProviderName + ".");
            }
        }

        public static string ApplyPaging(
            string baseSelectSql,
            ProviderFeatures features,
            string takeParameterName,
            string skipParameterName)
        {
            if (string.IsNullOrWhiteSpace(baseSelectSql))
                throw new ArgumentException("SQL non valido.", "baseSelectSql");

            if (features == null)
                throw new ArgumentNullException("features");

            string take = features.Parameter(takeParameterName);
            string skip = features.Parameter(skipParameterName);

            switch (features.LimitStyle)
            {
                case LimitStyle.Top:
                    if (features.SupportsOffsetPaging)
                    {
                        return baseSelectSql +
                               " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY";
                    }

                    return InsertTop(baseSelectSql, take);

                case LimitStyle.Limit:
                case LimitStyle.LimitOffset:
                    return baseSelectSql + " LIMIT " + take + " OFFSET " + skip;

                case LimitStyle.OffsetFetch:
                    return baseSelectSql +
                           " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY";

                case LimitStyle.FetchFirst:
                    return baseSelectSql +
                           " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY";

                case LimitStyle.Rows:
                    // Firebird ROWS è 1-based.
                    // Esempio: ROWS 1 TO 10.
                    // Con parametri conviene passare FromRow e ToRow già calcolati.
                    return baseSelectSql +
                           " ROWS " + features.Parameter("FromRow") +
                           " TO " + features.Parameter("ToRow");

                case LimitStyle.FirstSkip:
                    return InsertFirebirdFirstSkip(baseSelectSql, take, skip);

                case LimitStyle.RowNum:
                    throw new NotSupportedException(
                        "Paging completo con ROWNUM richiede una query Oracle dedicata con ROW_NUMBER().");

                default:
                    throw new NotSupportedException(
                        "Paging non supportato per provider " + features.ProviderName + ".");
            }
        }

        public static string BuildIdentitySql(
            ProviderFeatures features,
            string idColumnName)
        {
            if (features == null)
                throw new ArgumentNullException("features");

            if (string.IsNullOrWhiteSpace(idColumnName))
                idColumnName = "Id";

            string idColumn = features.QuoteIdentifier(idColumnName);

            switch (features.IdentityRetrievalStyle)
            {
                case IdentityRetrievalStyle.ScopeIdentity:
                    return "SELECT CAST(SCOPE_IDENTITY() AS int);";

                case IdentityRetrievalStyle.LastInsertId:
                    return "SELECT LAST_INSERT_ID();";

                case IdentityRetrievalStyle.LastInsertRowId:
                    return "SELECT last_insert_rowid();";

                case IdentityRetrievalStyle.Returning:
                    return " RETURNING " + idColumn;

                case IdentityRetrievalStyle.OutputInserted:
                    return " OUTPUT INSERTED." + idColumn;

                default:
                    throw new NotSupportedException(
                        "Recupero identity non supportato per provider " + features.ProviderName + ".");
            }
        }

        private static string InsertTop(string sql, string take)
        {
            string trimmedStart = sql.TrimStart();

            if (StartsWithIgnoreCase(trimmedStart, "SELECT DISTINCT "))
            {
                int absoluteIndex = sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
                int insertIndex = absoluteIndex + "SELECT DISTINCT ".Length;
                return sql.Insert(insertIndex, "TOP (" + take + ") ");
            }

            if (StartsWithIgnoreCase(trimmedStart, "SELECT "))
            {
                int absoluteIndex = sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
                int insertIndex = absoluteIndex + "SELECT ".Length;
                return sql.Insert(insertIndex, "TOP (" + take + ") ");
            }

            throw new NotSupportedException("La query deve iniziare con SELECT per poter applicare TOP.");
        }

        private static string InsertFirebirdFirstSkip(string sql, string take, string skip)
        {
            string trimmedStart = sql.TrimStart();

            if (!StartsWithIgnoreCase(trimmedStart, "SELECT "))
                throw new NotSupportedException("La query deve iniziare con SELECT per poter applicare FIRST/SKIP.");

            int absoluteIndex = sql.IndexOf("SELECT", StringComparison.OrdinalIgnoreCase);
            int insertIndex = absoluteIndex + "SELECT ".Length;

            string clause;

            if (string.IsNullOrEmpty(skip))
                clause = "FIRST " + take + " ";
            else
                clause = "FIRST " + take + " SKIP " + skip + " ";

            return sql.Insert(insertIndex, clause);
        }

        private static bool StartsWithIgnoreCase(string value, string prefix)
        {
            return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }
    }


}
