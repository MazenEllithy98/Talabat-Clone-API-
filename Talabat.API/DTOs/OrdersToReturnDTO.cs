using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.DTOs
{
    public class OrdersToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDTO> Items { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; }

    }
}