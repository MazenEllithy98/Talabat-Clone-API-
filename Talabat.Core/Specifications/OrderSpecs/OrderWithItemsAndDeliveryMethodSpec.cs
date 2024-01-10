using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderWithItemsAndDeliveryMethodSpec : BaseSpecification<Order>
    {
        public OrderWithItemsAndDeliveryMethodSpec(string Email)
            :base(O => O.BuyerEmail == Email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);

            AddOrderByDescending(O => O.OrderDate);
        }

        public OrderWithItemsAndDeliveryMethodSpec(string Email , int orderID)
    : base(O => O.BuyerEmail == Email && O.Id == orderID)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
        }

    }
}
