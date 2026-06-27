using Passero.Framework;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Passero.Framework.QueryEngine
{
    public interface IQueryEngine
    {
        CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            SelectQueryRequest<TModel> request = null)
            where TModel : class;

        CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            params Expression<Func<TModel, object?>>[] selectColumns)
            where TModel : class;

        CompiledQuery BuildSelect<TModel>(
            ViewModel<TModel> viewModel,
            IEnumerable<QueryConditionNode<TModel>> conditions,
            params Expression<Func<TModel, object?>>[] selectColumns)
            where TModel : class;

        CompiledQuery BuildInsert<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class;

        CompiledQuery BuildUpdate<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class;

        CompiledQuery BuildDelete<TModel>(
            ViewModel<TModel> viewModel,
            MutationQueryRequest<TModel> request = null)
            where TModel : class;

        Task<CompiledQuery> BuildSelectAsync<TModel>(
            ViewModel<TModel> viewModel,
            SelectQueryRequest<TModel> request = null,
            CancellationToken cancellationToken = default)
            where TModel : class;
    }
}