using Ai.Courses.Data.Repositories.Interfaces;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetItems;

public class GetItemsQueryHandler(IItemRepository itemRepository, IMapper mapper)
    : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
{
    public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await itemRepository.GetAllByUserIdAsync(request.UserId, request.DateFrom, request.DateTo, cancellationToken);
        return mapper.Map<IEnumerable<ItemDto>>(items);
    }
}
