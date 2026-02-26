using System.Security.Claims;
using MediatR;

namespace Ai.Courses.Logic.Commands.Login;

public class LoginCommand : IRequest<ClaimsPrincipal?>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
