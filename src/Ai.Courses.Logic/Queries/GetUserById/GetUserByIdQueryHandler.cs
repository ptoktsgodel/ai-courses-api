using Ai.Courses.Data.Entities;
using Ai.Courses.Logic.Models;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ai.Courses.Logic.Queries.GetUserById;

public class GetUserByIdQueryHandler(UserManager<UserEntity> userManager, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        return user is null ? null : mapper.Map<UserDto>(user);
    }
}
