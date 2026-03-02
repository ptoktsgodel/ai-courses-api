using Ai.Courses.Data.Repositories.Interfaces;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetTypes;

public class GetTypesQueryHandler(ITypeRepository typeRepository)
    : IRequestHandler<GetTypesQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await typeRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);
        return types.Select(t => t.Name);
    }
}
