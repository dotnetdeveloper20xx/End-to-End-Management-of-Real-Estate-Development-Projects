using BuildEstate.Domain.Common;
using BuildEstate.Shared.Pagination;
using System.Linq.Expressions;

namespace BuildEstate.Application.Common.Repositories;

public interface IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);
}