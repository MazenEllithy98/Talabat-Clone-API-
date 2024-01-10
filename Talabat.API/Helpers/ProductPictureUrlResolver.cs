using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entities;

namespace Talabat.API.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configure;

        public ProductPictureUrlResolver(IConfiguration configure)
        {
            _configure = configure;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureURL))
            {
                return $"{_configure["ApiBaseURL"]}{source.PictureURL}";
            }
            else return string.Empty;
        }
    }
}
