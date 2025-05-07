using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class WarehouseStatusDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int ItemCount { get; set; }
    }
}
