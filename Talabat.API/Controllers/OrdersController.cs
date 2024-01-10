using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.API.Controllers
{
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrdersToReturnDTO>> CreateOrder(OrdersDTO ordersDTO)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var ShippingAddress = _mapper.Map<AddressDTO, Address>(ordersDTO.ShippingAddress); 


            var order = await _orderService.CreateOrderAsync(buyerEmail, ordersDTO.BasketId, ordersDTO.DeliveryMethodId, ShippingAddress);

            if (order is null) return BadRequest(new ApiErrorResponse(400));
            return Ok(_mapper.Map<Order , OrdersToReturnDTO> (order));
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrdersToReturnDTO>>> GetOrderForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUsersAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrdersToReturnDTO>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersToReturnDTO>> GetOrderForUser(int id)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdForUserAsync(Email, id);

            if (order is null) return NotFound(new ApiErrorResponse(404));
            
            return Ok(_mapper.Map<Order,OrdersToReturnDTO>(order));
        }

        [HttpGet("deliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var deliveryMethod = await _orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethod);
        }


    }
}
