using Core.Primitives.Entity;

namespace Application.Abstractions.Database.Types;

public class PaginatedResult<TEntity>(ICollection<TEntity> Data, PaginationInfo Pagination)
    where TEntity : Entity
{
    public ICollection<TEntity> Data { get; set; } = Data;
    public PaginationInfo Pagination { get; set; } = Pagination;
}
