using System;
using System.Collections.Generic;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Repositories.Product2Repository.Validation
{
    public class Product2Validator : AbstractValidator<Product2>
    {
        public Product2Validator()
        {
        }
    }
}
