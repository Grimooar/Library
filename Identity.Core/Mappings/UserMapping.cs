using AutoMapper;
using Identity.Domain;
using Identity.DTOs;

namespace Identity.Core.Mappings;

public class UserMapping : Profile
{
    public UserMapping() {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}