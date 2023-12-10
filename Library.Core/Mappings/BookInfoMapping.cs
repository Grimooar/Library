using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class BookInfoMapping : Profile
{
    
    public BookInfoMapping() {
            CreateMap<BookInfo, BookInfoDto>().ReverseMap();
            CreateMap<BookInfo, BookInfoCreateDto>().ReverseMap();
            CreateMap<BookInfo, BookInfoUpdateDto>().ReverseMap();
        }
}