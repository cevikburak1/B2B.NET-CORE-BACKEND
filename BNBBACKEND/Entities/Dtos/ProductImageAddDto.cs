using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ProductImageAddDto
    {
        public int ProductId { get; set; }
        public IFormFile[] Images { get; set; }
    }
}
