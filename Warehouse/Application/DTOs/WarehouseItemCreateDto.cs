using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class WarehouseItemCreateDto
    {
        public string ItemName { get; set; }
        public string SKUCode { get; set; }
        public int Qty { get; set; }
        public decimal CostPrice { get; set; }
        public decimal? MSRPPrice { get; set; }
        public int WarehouseId { get; set; }
    }
}
