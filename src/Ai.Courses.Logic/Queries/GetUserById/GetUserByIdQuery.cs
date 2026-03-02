using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public string Id { get; set; }
}
