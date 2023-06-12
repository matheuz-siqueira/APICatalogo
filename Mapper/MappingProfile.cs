using APICatalogo.DTOs;
using APICatalogo.DTOs.User;
using AutoMapper;

namespace APICatalogo.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {   
        RequestForEntity();
        EntityForResponse();
    }

    private void RequestForEntity()
    {
        CreateMap<RequestCreateUserJson, Models.User>()
            .ForMember(destiny => destiny.Password, config => config.Ignore());
        
        CreateMap<RequestLoginJson, Models.User>();
        CreateMap<CategoryDto, Models.Category>().ReverseMap(); 
        CreateMap<ProductDto, Models.Product>().ReverseMap(); 
    }

    private void EntityForResponse()
    {
        CreateMap<Models.User, ResponseCreateUserJson>(); 
        CreateMap<Models.User, ResponseLoginJson>();
    }
}
