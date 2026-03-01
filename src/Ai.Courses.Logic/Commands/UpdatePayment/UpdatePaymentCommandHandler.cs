using Ai.Courses.Data.Entities;
using Ai.Courses.Data.Repositories.Interfaces;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;

namespace Ai.Courses.Logic.Commands.UpdatePayment;

public class UpdatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    ITypeRepository typeRepository,
    IMapper mapper) : IRequestHandler<UpdatePaymentCommand, PaymentDto?>
{
    public async Task<PaymentDto?> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetWithItemAsync(
            request.PaymentId, request.ItemId, request.UserId, cancellationToken);

        if (payment is null)
            return null;

        var type = await typeRepository.FindByNameAsync(request.UserId, request.TypeName, cancellationToken);

        if (type is null)
        {
            type = new TypeEntity { Id = Guid.NewGuid(), UserId = request.UserId, Name = request.TypeName };
            await typeRepository.AddAsync(type, cancellationToken);
        }

        payment.TypeId = type.Id;
        payment.PlannedAmount = request.PlannedAmount;
        payment.SpentAmount = request.SpentAmount;

        await paymentRepository.UpdateAsync(payment, cancellationToken);

        var updated = await paymentRepository.GetWithTypeAsync(payment.Id, cancellationToken);

        return mapper.Map<PaymentDto>(updated);
    }
}
