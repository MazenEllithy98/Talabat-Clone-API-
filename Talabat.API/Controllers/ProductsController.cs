using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IGenericRepository<Product> _productsRepository;
        //private readonly IGenericRepository<ProductBrand> _brandRepo;
        //private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            //IGenericRepository<Product> ProductsRepository  , 
            //IGenericRepository<ProductBrand> BrandRepo , 
            //IGenericRepository<ProductType> TypeRepo ,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_productsRepository = ProductsRepository;
            //_brandRepo = BrandRepo;
            //_typeRepo = TypeRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
     
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery]ProductSpecificationParameter productSpecParams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productSpecParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            var CountSpec = new ProductWithFilterationForCountSpecification(productSpecParams);

            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            return Ok(new Pagination<ProductToReturnDTO>(productSpecParams.PageIndex , productSpecParams.PageSize , count ,data));
        }
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            if (product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(_mapper.Map<Product,ProductToReturnDTO>(product));
        }

        [HttpGet("brands")] // Get : api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            
            return Ok(brands);
        }

        [HttpGet("types")] // Get : api/products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(types);
        }

    }
}
