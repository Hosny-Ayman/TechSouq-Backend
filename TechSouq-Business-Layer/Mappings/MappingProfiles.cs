using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSouq.Application.Dtos;
using TechSouq.Application.Services;
using TechSouq.Domain.Entities;

namespace TechSouq.Application.Mappings
{
    public class MappingProfiles:Profile
    {

        public MappingProfiles ()
        {
            CreateMap<CartItemDto, CartItem>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<CartDto,Cart>().ReverseMap();
            CreateMap<CategorieDto,Categorie>().ReverseMap();
            CreateMap<DeliveryMethodDto,DeliveryMethod>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();
        }

    }
}
