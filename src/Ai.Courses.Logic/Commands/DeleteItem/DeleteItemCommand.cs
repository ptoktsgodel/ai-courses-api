using MediatR;

namespace Ai.Courses.Logic.Commands.DeleteItem;

public class DeleteItemCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
}
