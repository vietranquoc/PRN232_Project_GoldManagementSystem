using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class CartCheckoutDTO
    {
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string? ReceiverEmail { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? ShippingMethod { get; set; }
    }
}
