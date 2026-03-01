using Ai.Courses.Data.Contexts;
using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;

using Ai.Courses.Data.Repositories.Interfaces;

namespace Ai.Courses.Data.Repositories;

public class PaymentRepository(PaymentDbContext db) : IPaymentRepository
{
    public async Task<PaymentEntity?> GetWithItemAsync(Guid paymentId, Guid itemId, Guid userId, CancellationToken cancellationToken)
        => await db.Payments
            .Include(p => p.Item)
            .FirstOrDefaultAsync(p =>
                p.Id == paymentId &&
                p.ItemId == itemId &&
                p.Item.UserId == userId,
                cancellationToken);

    public async Task<PaymentEntity> GetWithTypeAsync(Guid paymentId, CancellationToken cancellationToken)
        => await db.Payments
            .AsNoTracking()
            .Include(p => p.Type)
            .FirstAsync(p => p.Id == paymentId, cancellationToken);

    public async Task<int> CountByItemIdAsync(Guid itemId, CancellationToken cancellationToken)
        => await db.Payments.CountAsync(p => p.ItemId == itemId, cancellationToken);

    public async Task AddAsync(PaymentEntity payment, CancellationToken cancellationToken)
    {
        await db.Payments.AddAsync(payment, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken)
        => await db.SaveChangesAsync(cancellationToken);

    public async Task DeleteAsync(PaymentEntity payment, CancellationToken cancellationToken)
    {
        db.Payments.Remove(payment);
        await db.SaveChangesAsync(cancellationToken);
    }
}
