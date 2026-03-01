using Ai.Courses.Data.Repositories.Interfaces;
using MediatR;

namespace Ai.Courses.Logic.Commands.DeletePayment;

public class DeletePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IItemRepository itemRepository) : IRequestHandler<DeletePaymentCommand, bool>
{
    public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetWithItemAsync(
            request.PaymentId, request.ItemId, request.UserId, cancellationToken);

        if (payment is null)
            return false;

        await paymentRepository.DeleteAsync(payment, cancellationToken);

        var remainingPayments = await paymentRepository.CountByItemIdAsync(request.ItemId, cancellationToken);

        if (remainingPayments == 0)
        {
            var item = await itemRepository.GetByIdAsync(request.ItemId, request.UserId, cancellationToken);
            if (item is not null)
                await itemRepository.DeleteAsync(item, cancellationToken);
        }

        return true;
    }
}
