using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Passero.Framework.Controls;

public sealed class QueryBuilderRuleSet
{
    [JsonPropertyName("condition")]
    public string Condition { get; set; } = "and";

    [JsonPropertyName("rules")]
    public List<QueryBuilderRuleNode> Rules { get; set; } = new List<QueryBuilderRuleNode>();
}

public sealed class QueryBuilderRuleNode
{
    [JsonPropertyName("condition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Condition { get; set; }

    [JsonPropertyName("rules")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<QueryBuilderRuleNode>? Rules { get; set; }

    [JsonPropertyName("field")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Field { get; set; }

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonPropertyName("operator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Operator { get; set; }

    [JsonPropertyName("value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Value { get; set; }

    /// <summary>Secondo valore per operatori Between / Not Between.</summary>
    [JsonPropertyName("value2")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Value2 { get; set; }

    [JsonIgnore]
    public bool IsGroup => Rules is not null;
}
