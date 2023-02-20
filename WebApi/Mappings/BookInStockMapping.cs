using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings;

public class BookInStockMapping : Profile
{
    public BookInStockMapping() {
        CreateMap<BookInStock, BookInStockDto>().ReverseMap();
        CreateMap<BookInStock, BookInStockCreateDto>().ReverseMap();
        CreateMap<BookInStock, BookInStockUpdateDto>().ReverseMap();
    }
}