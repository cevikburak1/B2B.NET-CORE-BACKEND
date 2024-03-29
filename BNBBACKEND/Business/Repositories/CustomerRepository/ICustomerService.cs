using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;
using Entities.Dtos;

namespace Business.Repositories.CustomerRepository
{
    public interface ICustomerService
    {
        Task<IResult> Add(CustomerRegisterDto customerRegisterDto);
        Task<IResult> Update(Customer customer);
        Task<IResult> ChangePasswordByAdmin(CustomerChangePasswordByAdminDto customerDto);
        Task<IResult> Delete(Customer customer);
        Task<IDataResult<List<CustomerDto>>> GetList();
        Task<IDataResult<Customer>> GetById(int id);

        Task<IDataResult<CustomerDto>> GetDtoById(int id);
        Task<Customer> GetByEmail(string email);


    }
}
