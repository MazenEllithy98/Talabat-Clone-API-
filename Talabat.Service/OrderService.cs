using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethods;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            //IGenericRepository<Product> ProductRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethods , 
            //IGenericRepository<Order> orderRepo
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //    _productRepo = ProductRepo;
            //    _deliveryMethods = deliveryMethods;
            //    _orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string BasketId, int DeliveryMethodId, Address shippingAddress)
        {
            /// 1. Get Basket from Basket Repo
            
            var basket = await _basketRepository.GetBasketAsync(BasketId);
            /// 2. Get Selected Items at Basket from Products Repo
             var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
                {

                    foreach (var item in basket.Items)
                    {
                        var productsRepo = _unitOfWork.Repository<Product>();
                        if (productsRepo is not null)
                        {
                            var product = await productsRepo.GetByIdAsync(item.Id);
                            if (product is not null)
                            {
                                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureURL);
                                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                                OrderItems.Add(orderItem);
                            }
                        }

                    }
                }

            /// 3. Calculate SubTotal 
            var subtotal = OrderItems.Sum(item => item.Cost * item.Quantity);
                /// 4. Get Delivery Method from Delivery Methods Repo 

                DeliveryMethod deliveryMethod = new DeliveryMethod();
                var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();
            
                if (deliveryMethodsRepo is not null)
                 deliveryMethod = await deliveryMethodsRepo.GetByIdAsync(DeliveryMethodId);
            /// 5. Create Order
            /// 
            var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntentId);


            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, OrderItems, subtotal , basket.PaymentIntentId);

            var ordersRepo = _unitOfWork.Repository<Order>();
                if (ordersRepo is not null)
                {
                    await ordersRepo.Add(order);
                    /// 6. Save to Database
                    var result = await _unitOfWork.Complete();
                    if (result > 0)
                        return order;
                }

            return null;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string buyerEmail)
        {            
            var ordersRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithItemsAndDeliveryMethodSpec(buyerEmail);

            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpec(buyerEmail, OrderId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods= await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;
        }
    }
}
