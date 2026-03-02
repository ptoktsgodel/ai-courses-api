using Ai.Courses.Data.Entities;
using Ai.Courses.Logic.Models;
using AutoMapper;

namespace Ai.Courses.Logic.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserDto, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
