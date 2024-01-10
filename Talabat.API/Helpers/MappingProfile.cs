using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(D=>D.ProductBrand , O=>O.MapFrom(s=>s.productBrand.Name))
                .ForMember(d=>d.ProductType , o=> o.MapFrom(s=>s.productType.Name))
                .ForMember(d => d.PictureURL , o=> o.MapFrom <ProductPictureUrlResolver>());


            CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();

            CreateMap<CustomerBasketDTO, CustomerBasket>();

            CreateMap<BasketItemDTO , BasketItem>();

            CreateMap<AddressDTO, Core.Entities.Order_Aggregate.Address>();

            CreateMap<Order , OrdersToReturnDTO >()
                .ForMember(d => d.DeliveryMethod , O=> O.MapFrom(S=>S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost , O=> O.MapFrom(S=>S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(S => S.Product.PictureUrl));
                


        }
    }
}
