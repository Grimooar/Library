using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings;

public class BookAbonementMapping : Profile
{
    public BookAbonementMapping() {
        CreateMap<BookInAbonement, BookInAbonementDto>().ReverseMap();
        CreateMap<BookInAbonement, BookInAbonementUpdateDto>().ReverseMap();
        CreateMap<BookInAbonement, BookInAbonementCreateDto>().ReverseMap();
       
    }
    
}