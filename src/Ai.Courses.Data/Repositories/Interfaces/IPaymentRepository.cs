using Ai.Courses.Data.Entities;

namespace Ai.Courses.Data.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<PaymentEntity?> GetWithItemAsync(Guid paymentId, Guid itemId, Guid userId, CancellationToken cancellationToken);
    Task<PaymentEntity> GetWithTypeAsync(Guid paymentId, CancellationToken cancellationToken);
    Task<int> CountByItemIdAsync(Guid itemId, CancellationToken cancellationToken);
    Task AddAsync(PaymentEntity payment, CancellationToken cancellationToken);
    Task UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken);
    Task DeleteAsync(PaymentEntity payment, CancellationToken cancellationToken);
}
