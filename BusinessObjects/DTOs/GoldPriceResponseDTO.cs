using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GoldPriceResponseDTO
    {
        public int GoldTypeId { get; set; }
        public string GoldTypeName { get; set; }
        public string Description { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
