using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings;

public class BookInfoMapping : Profile
{
    
    public BookInfoMapping() {
            CreateMap<BookInfo, BookInfoDto>().ReverseMap();
            CreateMap<BookInfo, BookInfoCreateDto>().ReverseMap();
            CreateMap<BookInfo, BookInfoUpdateDto>().ReverseMap();
        }
}