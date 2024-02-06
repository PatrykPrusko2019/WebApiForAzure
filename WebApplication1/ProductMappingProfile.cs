using AutoMapper;
using WebApplication1.Entities;
using WebApplication1.Models;
using WebApplication1.Models.ShowAll;

namespace WebApplication1
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<ProductDto, DetailsProductDto>();

            CreateMap<InventoryDto, Inventory>();

            CreateMap<PriceDto, Price>();
            CreateMap<Price, PriceDto>();

            CreateMap<DetailsProductDto, ShowProductDto>();

            CreateMap<Product, AllProductDto>();

            CreateMap<Inventory, AllInventoryDto>();

            CreateMap<Price, AllPriceDto>();

            CreateMap<User, UserDto>();
        }
    }
}
