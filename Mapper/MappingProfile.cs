using APICatalogo.DTOs;
using APICatalogo.DTOs.Category;
using APICatalogo.DTOs.Product;
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
        CreateMap<RequestCreateCategoryJson, Models.Category>();
        CreateMap<RequestUpdateCategoryJson, Models.Category>().ReverseMap();
        CreateMap<RequestCreateProductJson, Models.Product>();
        CreateMap<RequestUpdateProductJson, Models.Product>();
         
    }

    private void EntityForResponse()
    {
        CreateMap<Models.User, ResponseCreateUserJson>(); 
        CreateMap<Models.User, ResponseLoginJson>();
        CreateMap<Models.Category, ResponseCategoryJson>();
        CreateMap<Models.Category, ResponseCategoryProductsJson>(); 
        CreateMap<Models.Product, ResponseProductJson>();
    }
}
