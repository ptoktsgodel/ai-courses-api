using Ai.Courses.Data.Contexts;
using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;

using Ai.Courses.Data.Repositories.Interfaces;

namespace Ai.Courses.Data.Repositories;

public class ItemRepository(PaymentDbContext db) : IItemRepository
{
    public async Task<IEnumerable<ItemEntity>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => await db.Items
            .AsNoTracking()
            .Where(i => i.UserId == userId)
            .Include(i => i.Payments)
                .ThenInclude(p => p.Type)
            .OrderBy(i => i.Date)
            .ToListAsync(cancellationToken);

    public async Task<ItemEntity?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => await db.Items
            .AsNoTracking()
            .Where(i => i.Id == id && i.UserId == userId)
            .Include(i => i.Payments)
                .ThenInclude(p => p.Type)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ItemEntity?> FindByDateAsync(Guid userId, DateTime date, CancellationToken cancellationToken)
        => await db.Items
            .Where(i => i.UserId == userId && i.Date == date)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<ItemEntity> GetWithPaymentsAsync(Guid id, CancellationToken cancellationToken)
        => await db.Items
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Include(i => i.Payments)
                .ThenInclude(p => p.Type)
            .FirstAsync(cancellationToken);

    public async Task AddAsync(ItemEntity item, CancellationToken cancellationToken)
    {
        await db.Items.AddAsync(item, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ItemEntity item, CancellationToken cancellationToken)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync(cancellationToken);
    }
}
