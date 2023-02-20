using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings;

public class BookMappings : Profile
{

    public BookMappings() {
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<Book, BookCreateDto>().ReverseMap();
        CreateMap<Book, BookUpdateDto>().ReverseMap();
    }
}

