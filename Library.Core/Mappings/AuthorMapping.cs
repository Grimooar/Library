using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class AuthorMapping : Profile
{
    public AuthorMapping() {
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Author, AuthorCreateDto>().ReverseMap();
        CreateMap<Author, AuthorUpdateDto>().ReverseMap();
       
    }
    
}