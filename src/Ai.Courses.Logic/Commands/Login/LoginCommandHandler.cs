using System.Security.Claims;
using Ai.Courses.Data.Entities;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ai.Courses.Logic.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ClaimsPrincipal?>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;

    public LoginCommandHandler(UserManager<UserEntity> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ClaimsPrincipal?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return null;

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid) return null;

        var dto = _mapper.Map<UserDto>(user);

        var identity = new ClaimsIdentity(IdentityConstants.BearerScheme);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, dto.Id));
        identity.AddClaim(new Claim(ClaimTypes.Email, dto.Email));
        identity.AddClaim(new Claim(ClaimTypes.Name, dto.Email));

        return new ClaimsPrincipal(identity);
    }
}
