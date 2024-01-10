using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.DTOs
{
    public class OrdersDTO
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
