using AutoMapper;
using IndevLabs.Models.db;
using IndevLabs.Models.Wines;

namespace IndevLabs.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Wine, WineDto>().ReverseMap();
    }
}