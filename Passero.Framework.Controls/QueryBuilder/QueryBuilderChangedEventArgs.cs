namespace Passero.Framework.Controls;

public sealed class QueryBuilderChangedEventArgs : System.EventArgs
{
    public QueryBuilderChangedEventArgs(QueryBuilderRuleSet rules)
    {
        Rules = rules;
    }

    public QueryBuilderRuleSet Rules { get; }
}
