using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.CustomerRelationShipsRepository
{
    public interface ICustomerRelationShipsService
    {
        Task<IResult> Add(CustomerRelationShips customerRelationShips);
        Task<IResult> Update(CustomerRelationShips customerRelationShips);
        Task<IResult> Delete(CustomerRelationShips customerRelationShips);
        Task<IDataResult<List<CustomerRelationShips>>> GetList();
        Task<IDataResult<CustomerRelationShips>> GetById(int id);
        Task<IDataResult<CustomerRelationShips>> GetByCustomerId(int customerId);
    }
}
