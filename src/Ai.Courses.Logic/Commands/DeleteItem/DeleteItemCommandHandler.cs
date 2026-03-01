using Ai.Courses.Data.Repositories.Interfaces;
using MediatR;

namespace Ai.Courses.Logic.Commands.DeleteItem;

public class DeleteItemCommandHandler(IItemRepository itemRepository) : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(request.ItemId, request.UserId, cancellationToken);

        if (item is null)
            return false;

        await itemRepository.DeleteAsync(item, cancellationToken);

        return true;
    }
}
