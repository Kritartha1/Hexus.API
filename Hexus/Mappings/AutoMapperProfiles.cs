using AutoMapper;
using Hexus.Models.Domain;
using Hexus.Models.DTO;

namespace Hexus.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<DM, DMDto>().ReverseMap();
            CreateMap<DM,AddDMRequestDto>().ReverseMap();
            CreateMap<Message,MessageDto>().ReverseMap();
           // CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
