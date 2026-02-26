using Ai.Courses.Logic.Models;
using MediatR;

namespace Ai.Courses.Logic.Commands.Register;

public class RegisterCommand : IRequest<UserDto?>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
