using Ai.Courses.Data.Entities;
using Ai.Courses.Data.Repositories.Interfaces;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;

namespace Ai.Courses.Logic.Commands.AddItemPayment;

public class AddItemPaymentCommandHandler(
    IItemRepository itemRepository,
    IPaymentRepository paymentRepository,
    ITypeRepository typeRepository,
    IMapper mapper) : IRequestHandler<AddItemPaymentCommand, ItemDto>
{
    public async Task<ItemDto> Handle(AddItemPaymentCommand request, CancellationToken cancellationToken)
    {
        var date = request.Date.Date;

        var item = await itemRepository.FindByDateAsync(request.UserId, date, cancellationToken);

        if (item is null)
        {
            item = new ItemEntity { Id = Guid.NewGuid(), UserId = request.UserId, Date = date };
            await itemRepository.AddAsync(item, cancellationToken);
        }

        var type = await typeRepository.FindByNameAsync(request.UserId, request.TypeName, cancellationToken);

        if (type is null)
        {
            type = new TypeEntity { Id = Guid.NewGuid(), UserId = request.UserId, Name = request.TypeName };
            await typeRepository.AddAsync(type, cancellationToken);
        }

        var payment = new PaymentEntity
        {
            Id = Guid.NewGuid(),
            ItemId = item.Id,
            TypeId = type.Id,
            PlannedAmount = request.PlannedAmount,
            SpentAmount = request.SpentAmount
        };

        await paymentRepository.AddAsync(payment, cancellationToken);

        var result = await itemRepository.GetWithPaymentsAsync(item.Id, cancellationToken);

        return mapper.Map<ItemDto>(result);
    }
}
