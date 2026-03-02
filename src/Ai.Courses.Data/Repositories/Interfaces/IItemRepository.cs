using Ai.Courses.Data.Entities;

namespace Ai.Courses.Data.Repositories.Interfaces;

public interface IItemRepository
{
    Task<IEnumerable<ItemEntity>> GetAllByUserIdAsync(Guid userId, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken);
    Task<ItemEntity?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<ItemEntity?> FindByDateAsync(Guid userId, DateTime date, CancellationToken cancellationToken);
    Task<ItemEntity> GetWithPaymentsAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(ItemEntity item, CancellationToken cancellationToken);
    Task DeleteAsync(ItemEntity item, CancellationToken cancellationToken);
}
