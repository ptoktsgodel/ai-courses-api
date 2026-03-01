using Ai.Courses.Data.Entities;
using Ai.Courses.Logic.Models;
using AutoMapper;

namespace Ai.Courses.Logic.Mappings;

public class PaymentsProfile : Profile
{
    public PaymentsProfile()
    {
        CreateMap<ItemEntity, ItemDto>();

        CreateMap<PaymentEntity, PaymentDto>()
            .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : string.Empty));
    }
}
