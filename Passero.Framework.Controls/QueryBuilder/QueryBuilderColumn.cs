using System.Collections.Generic;

namespace Passero.Framework.Controls;

public enum QueryBuilderFieldType
{
    String,
    Number,
    Boolean,
    Date,
    DateTime,
    Guid,
    Enum
}

public sealed class QueryBuilderColumn
{
    public string Field { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public QueryBuilderFieldType Type { get; set; } = QueryBuilderFieldType.String;
    public string? SqlFieldName { get; set; }
    public List<QueryBuilderOperator> Operators { get; set; } = new List<QueryBuilderOperator>();
    public List<QueryBuilderLookupItem> Values { get; set; } = new List<QueryBuilderLookupItem>();

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Label) ? Field : Label;
    }
}

/// <summary>
/// Descrive quanti/quali valori richiede un operatore di confronto.
/// </summary>
public enum OperatorValueMode
{
    /// <summary>Nessun valore (es. Is Null).</summary>
    None,
    /// <summary>Un singolo valore (es. Equal, Contains).</summary>
    Single,
    /// <summary>Due valori: limite inferiore e superiore (es. Between).</summary>
    Range,
    /// <summary>Lista di valori separati da virgola (es. In, Not In).</summary>
    List
}

public sealed class QueryBuilderOperator
{
    public string Key { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool RequiresValue { get; set; } = true;
    public OperatorValueMode ValueMode { get; set; } = OperatorValueMode.Single;

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Text) ? Key : Text;
    }
}

public sealed class QueryBuilderLookupItem
{
    public object? Value { get; set; }
    public string Text { get; set; } = string.Empty;

    public override string ToString()
    {
        return Text;
    }
}
