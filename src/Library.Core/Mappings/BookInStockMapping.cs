using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class BookInStockMapping : Profile
{
    public BookInStockMapping() {
        CreateMap<BookInStock, BookInStockDto>().ReverseMap();
        CreateMap<BookInStock, BookInStockCreateDto>().ReverseMap();
        CreateMap<BookInStock, BookInStockUpdateDto>().ReverseMap();
    }
}