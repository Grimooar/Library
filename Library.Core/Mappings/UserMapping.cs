using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class UserMapping : Profile
{
    public UserMapping() {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}