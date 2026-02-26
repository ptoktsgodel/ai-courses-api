using Ai.Courses.Data.Entities;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ai.Courses.Logic.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto?>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(UserManager<UserEntity> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = new UserDto
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var user = _mapper.Map<UserEntity>(dto);
        user.UserName = request.Email;

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) return null;

        return _mapper.Map<UserDto>(user);
    }
}
