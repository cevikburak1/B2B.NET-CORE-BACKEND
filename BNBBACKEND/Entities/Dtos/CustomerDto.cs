using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CustomerDto:Customer
    {
        public int? PrticeListId { get; set; }
        public string PrticeListName { get; set; }
        public decimal? Discount { get; set; }
    }
}
