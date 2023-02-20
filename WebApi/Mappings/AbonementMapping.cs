using AutoMapper;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Mappings;

public class AbonementMapping : Profile
{
    public AbonementMapping()
    {
        CreateMap<Abonement, AbonementDto>().ReverseMap();
        CreateMap<Abonement, AbonementCreateDto>().ReverseMap();
        CreateMap<Abonement, AbonementUpdateDto>().ReverseMap();

    }
}