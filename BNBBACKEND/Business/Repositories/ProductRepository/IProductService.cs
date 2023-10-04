using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.ProductRepository
{
    public interface IProductService
    { 
        Task<IResult> Add(Product product);
        Task<IResult> Update(Product product);
        Task<IResult> Delete(Product product);
        Task<IDataResult<List<ProductListDto>>> GetList();

        Task<IDataResult<List<ProductListDto>>> GetProductList(int customerId);
        Task<IDataResult<Product>> GetById(int id);
    }
}
