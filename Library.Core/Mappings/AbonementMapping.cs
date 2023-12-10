using AutoMapper;
using ClassLibrary1;
using Library.Models;

namespace Library.Core.Mappings;

public class AbonementMapping : Profile
{
    public AbonementMapping()
    {
        CreateMap<Abonement, AbonementDto>().ReverseMap();
        CreateMap<Abonement, AbonementCreateDto>().ReverseMap();
        CreateMap<Abonement, AbonementUpdateDto>().ReverseMap();

    }
}