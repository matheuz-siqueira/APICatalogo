using APICatalogo.DTOs;
using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
