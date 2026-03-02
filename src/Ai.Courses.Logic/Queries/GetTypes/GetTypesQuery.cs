using MediatR;

namespace Ai.Courses.Logic.Queries.GetTypes;

public class GetTypesQuery : IRequest<IEnumerable<string>>
{
    public Guid UserId { get; set; }
}
