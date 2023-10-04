using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.Product2Repository
{
    public interface IProduct2Service
    {
        Task<IResult> Add(Product2 product2);
        Task<IResult> Update(Product2 product2);
        Task<IResult> Delete(Product2 product2);
        Task<IDataResult<List<Product2>>> GetList();
        Task<IDataResult<Product2>> GetById(int id);
    }
}
