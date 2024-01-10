using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string BasketId, int DeliveryMethodId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUsersAsync(string buyerEmail);

        Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int OrderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
