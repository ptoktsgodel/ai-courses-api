using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetItems;

public class GetItemsQuery : IRequest<IEnumerable<ItemDto>>
{
    public Guid UserId { get; set; }
}
