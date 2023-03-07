using AutoMapper;
using ClassLibrary1;
using WebApi.Models;

namespace WebApi.Mappings;

public class AuthorMapping : Profile
{
    public AuthorMapping() {
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Author, AuthorCreateDto>().ReverseMap();
        CreateMap<Author, AuthorUpdateDto>().ReverseMap();
       
    }
    
}