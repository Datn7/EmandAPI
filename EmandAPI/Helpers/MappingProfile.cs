using AutoMapper;
using EmandAPI.Models.DTOs;
using EmandAPI.Models.Entities;

namespace EmandAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, User>();
            CreateMap<User, ProfileDto>();

            CreateMap<Claim, ClaimDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => src.SubmittedAt))
            .ReverseMap();
        }
    }
}
