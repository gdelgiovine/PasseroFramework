using Dapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Passero.Framework.QueryEngine
{
    public enum QueryLogicalCondition
    {
        And = 0,
        Or = 1
    }

    public abstract class QueryConditionNode<TModel> where TModel : class
    {
    }

    public sealed class QueryFilter<TModel> : QueryConditionNode<TModel> where TModel : class
    {
        public Expression<Func<TModel, object?>> Column { get; set; } = null!;

        public QueryOperator Operator { get; set; } = QueryOperator.Equal;

        public object? Value { get; set; }

        public object? Value2 { get; set; }
    }

    public sealed class QueryConditionGroup<TModel> : QueryConditionNode<TModel> where TModel : class
    {
        public QueryLogicalCondition Condition { get; set; } = QueryLogicalCondition.And;

        public List<QueryConditionNode<TModel>> Conditions { get; } = new List<QueryConditionNode<TModel>>();
    }

    public sealed class SelectQueryRequest<TModel> where TModel : class
    {
        public List<Expression<Func<TModel, object?>>> SelectColumns { get; } =
            new List<Expression<Func<TModel, object?>>>();

        public List<QueryConditionNode<TModel>> Conditions { get; } =
            new List<QueryConditionNode<TModel>>();

        [Obsolete("Use Conditions instead.")]
        public List<QueryConditionNode<TModel>> Filters => Conditions;

        public List<QueryOrder<TModel>> OrderBy { get; } = new List<QueryOrder<TModel>>();

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public bool Distinct { get; set; }

        public bool UseDefaultOrderByClause { get; set; } = true;
    }

    public class MutationQueryRequest<TModel> where TModel : class
    {
        public List<QueryConditionNode<TModel>> Conditions { get; } =
            new List<QueryConditionNode<TModel>>();

        [Obsolete("Use Conditions instead.")]
        public List<QueryConditionNode<TModel>> Filters => Conditions;

        public Expression<Func<TModel, object?>>[]? KeyColumns { get; set; }
    }

    public sealed class QueryOrder<TModel> where TModel : class
    {
        public Expression<Func<TModel, object?>> Column { get; set; } = null!;

        public bool Descending { get; set; }
    }

    public enum QueryOperator
    {
        Equal = 0,
        NotEqual = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThan = 4,
        LessThanOrEqual = 5,
        Like = 6,
        NotLike = 7,
        Contains = 8,
        StartsWith = 9,
        EndsWith = 10,
        In = 11,
        NotIn = 12,
        Between = 13,
        NotBetween = 14,
        IsNull = 15,
        IsNotNull = 16
    }

    public sealed class CompiledQuery
    {
        public CompiledQuery(string sql, DynamicParameters parameters)
        {
            Sql = sql ?? string.Empty;
            Parameters = parameters ?? new DynamicParameters();
        }

        public string Sql { get; }

        public DynamicParameters Parameters { get; }
    }
}