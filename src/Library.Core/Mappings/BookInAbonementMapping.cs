using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class BookAbonementMapping : Profile
{
    public BookAbonementMapping() {
        CreateMap<BookInAbonement, BookInAbonementDto>().ReverseMap();
        CreateMap<BookInAbonement, BookInAbonementUpdateDto>().ReverseMap();
        CreateMap<BookInAbonement, BookInAbonementCreateDto>().ReverseMap();
       
    }
    
}