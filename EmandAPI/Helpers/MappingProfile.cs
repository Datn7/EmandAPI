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
        }
    }
}
