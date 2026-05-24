using System;
using System.Collections.Generic;

namespace Passero.Framework.Controls.Models
{
    public abstract class QueryNode
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public bool Locked { get; set; }
    }

    public class QueryRule : QueryNode
    {
        public string Field { get; set; }

        public string Operator { get; set; }

        public object Value { get; set; }
    }

    public class QueryGroup : QueryNode
    {
        public string Condition { get; set; } = "AND";

        public List<QueryNode> Rules { get; set; } = new List<QueryNode>();
    }

    public class QueryBuilderColumnX
    {
        public string Field { get; set; }

        public string Label { get; set; }

        public Type DataType { get; set; }

        public List<string> Values { get; set; }
    }

    public class SqlQueryResult
    {
        public string WhereClause { get; set; }

        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
