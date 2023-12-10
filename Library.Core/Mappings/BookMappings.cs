using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class BookMappings : Profile
{

    public BookMappings() {
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<Book, BookCreateDto>().ReverseMap();
        CreateMap<Book, BookUpdateDto>().ReverseMap();
    }
}

