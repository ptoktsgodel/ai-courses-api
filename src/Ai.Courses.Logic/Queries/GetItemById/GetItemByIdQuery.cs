using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetItemById;

public class GetItemByIdQuery : IRequest<ItemDto?>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
