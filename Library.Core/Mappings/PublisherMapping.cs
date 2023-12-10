using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class PublisherMapping : Profile
{
    public PublisherMapping() {
        CreateMap<Publisher, PublisherDto>().ReverseMap();
        CreateMap<Publisher, PublisherCreateDto>().ReverseMap();
        CreateMap<Publisher, PublisherUpdateDto>().ReverseMap();
    }
}