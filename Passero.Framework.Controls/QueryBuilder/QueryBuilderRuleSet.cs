using System.Collections.Generic;
using Newtonsoft.Json;

namespace Passero.Framework.Controls;

public sealed class QueryBuilderRuleSet
{
    [JsonProperty("condition")]
    public string Condition { get; set; } = "and";

    [JsonProperty("rules")]
    public List<QueryBuilderRuleNode> Rules { get; set; } = new List<QueryBuilderRuleNode>();
}

public sealed class QueryBuilderRuleNode
{
    [JsonProperty("condition")]
    public string? Condition { get; set; }

    [JsonProperty("rules")]
    public List<QueryBuilderRuleNode>? Rules { get; set; }

    [JsonProperty("field")]
    public string? Field { get; set; }

    [JsonProperty("label")]
    public string? Label { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("operator")]
    public string? Operator { get; set; }

    [JsonProperty("value")]
    public object? Value { get; set; }

    /// <summary>Secondo valore per operatori Between / Not Between.</summary>
    [JsonProperty("value2")]
    public object? Value2 { get; set; }

    [JsonIgnore]
    public bool IsGroup => Rules is not null;
}
