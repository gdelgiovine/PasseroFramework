using System.Collections.Generic;

namespace Passero.Framework.Controls;

public sealed class QueryBuilderSqlResult
{
    public string WhereClause { get; set; } = string.Empty;
    public Dictionary<string, object?> Parameters { get; set; } = new();
}
