using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetItems;

public class GetItemsQuery : IRequest<IEnumerable<ItemDto>>
{
    public Guid UserId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
