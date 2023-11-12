using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class OrderDto:Order
    {
        public string CustomeName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
