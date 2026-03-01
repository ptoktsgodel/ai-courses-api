using Ai.Courses.Data.Repositories.Interfaces;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetItemById;

public class GetItemByIdQueryHandler(IItemRepository itemRepository, IMapper mapper)
    : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken);
        return item is null ? null : mapper.Map<ItemDto>(item);
    }
}
